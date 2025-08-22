using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class AudioVolumeAnalyzer : MonoBehaviour
{
    [Header("Refs")]
    public AudioSource source;
    public AudioDistortionFilter distortionFilter;
    public Sprite specificRippleSprite;
    public SoundLight rippleManager;

    [Header("Flow & Scene")]
    public float waitTime = 8f;          // 재생 지연(초) – DSP 시계 기준
    public float startSilenceAt = 50f;   // 재생 후 몇 초에 페이드 아웃 시작할지
    public float changeSceneAt = 63f;    // 재생 후 몇 초에 씬 전환할지
    public string nextSceneName = "Hospital_1F";

    [Header("Detection (audio-thread)")]
    [Tooltip("분석 창 길이(초). 샘플레이트에 맞춰 자동 환산됨")]
    [Range(0.01f, 0.2f)] public float analysisWindowSec = 0.0464f; // ~2048/44.1k
    [Tooltip("적응형 임계치 계수 (EMA * K)")]
    [Range(0.5f, 5f)] public float thresholdK = 2.0f;
    [Tooltip("이동 평균의 시간 상수(초). 클수록 완만")]
    [Range(0.05f, 1.0f)] public float emaTauSec = 0.3f;

    [Header("Spawning (main-thread)")]
    [Tooltip("스폰 쿨다운(초). DSP 시계 기준 유지")]
    [Range(0.01f, 0.5f)] public float spawnCooldownSec = 0.10f;
    [Tooltip("리플 랜덤 오프셋 반경")]
    public float rippleRadius = 10f;

    // --- DSP & 분석 상태 ---
    private int sampleRate;
    private int windowSamples;
    private double startDspTime = -1;    // PlayScheduled로 예약한 절대 시작 시각
    private long processedSamples = 0;   // OnAudioFilterRead에서 처리한 샘플 누계(채널당)
    private double ema = 0.0;            // |신호|의 EMA
    private double emaAlpha = 0.2;       // analysisWindowSec와 emaTauSec으로부터 계산

    // 오디오스레드→메인스레드 이벤트 전달 (잠금으로 간단 동기화)
    private readonly object eventLock = new object();
    private double lastDetectionDsp = -1.0; // 마지막 “감지” 시각(오디오 스레드에서 기록)
    private double lastSpawnDsp = -999.0;   // 마지막 “스폰” 시각(메인 스레드에서 기록)

    private bool silenceStarted = false;
    private bool Stop = false;

    public GameObject AudioSource;
    public BeforeSceneEnd beforeEnd;

    void Awake()
    {
        if (!source) source = GetComponent<AudioSource>();
        if (source) source.playOnAwake = false;
        sampleRate = AudioSettings.outputSampleRate;

        windowSamples = Mathf.Max(256, Mathf.RoundToInt(sampleRate * analysisWindowSec));
        // EMA 알파를 창길이/시간상수로부터 계산 (연속시간 1차 저역통과 근사)
        emaAlpha = 1.0 - Math.Exp(-(analysisWindowSec / Math.Max(1e-3f, emaTauSec)));

        if (source) source.volume = 1f;
    }

    void Start()
    {
        // DSP 시계 기준으로 재생 예약
        double now = AudioSettings.dspTime;
        startDspTime = now + waitTime;
        if (source && source.clip)
            source.PlayScheduled(startDspTime);
    }

    void Update()
    {
        if (startDspTime < 0 || source == null) return;
        Stop = AudioListener.pause;

        double dspNow = AudioSettings.dspTime;
        double playhead = dspNow - startDspTime; // 재생 기준 경과 시간(초)

        // 사일런스/씬 전환도 DSP 기준으로
        if (!silenceStarted && playhead >= startSilenceAt)
        {
            silenceStarted = true;
            StartCoroutine(FadeOutVolume(10f));
        }

        if (playhead >= changeSceneAt)
        {
            // 프로젝트별 사용자 코드 유지
            beforeEnd.BeforeSceneEndPanel.SetActive(true);
            StartCoroutine(beforeEnd.Sequence());
            SceneManage.Instance.entryPointID = 0;
            SceneManager.LoadScene(nextSceneName);
            return;
        }
        if (!Stop)
        {
            // 오디오 스레드에서 신호 감지되면, DSP 시계 기준 쿨다운으로 스폰
            double detectedAt;
            lock (eventLock) detectedAt = lastDetectionDsp;

            if (detectedAt > 0 && (dspNow - lastSpawnDsp) >= spawnCooldownSec)
            {
                // 재현성 있는 랜덤: “감지 시각(초)×샘플레이트”로 시드 파생
                int seed = Mathf.Abs((int)(detectedAt * sampleRate));
                var rand = new System.Random(seed);
                Vector3 dir = new Vector3(
                    (float)(rand.NextDouble() * 2 - 1),
                    (float)(rand.NextDouble() * 2 - 1),
                    (float)(rand.NextDouble() * 2 - 1)
                ).normalized;
                Vector3 offset = dir * rippleRadius * (float)rand.NextDouble();
                Vector3 range = new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-5f, 5f), 0);
                if (rippleManager != null)
                {
                    
                    rippleManager.PlayRippleEffect(AudioSource.transform.position + range, source.maxDistance, null);
                }

                lock (eventLock) lastSpawnDsp = detectedAt;
            }
        }
    }
    
    // 오디오스레드: 출력 버퍼마다 호출됨. 여기서 신호 세기를 분석.
    void OnAudioFilterRead(float[] data, int channels)
    {
        if (startDspTime < 0 || data == null || channels <= 0) return;
        if (Stop) return;

        int n = data.Length / channels;          // 채널당 샘플 수
        if (n <= 0) return;

        // 이 청크 중앙의 “재생될 DSP 시각”을 추정
        // (PlayScheduled 기준으로 누적 샘플 -> 절대 DSP 시각으로 변환)
        double chunkStartDsp = startDspTime + (double)processedSamples / sampleRate;
        double chunkCenterDsp = chunkStartDsp + 0.5 * (double)n / sampleRate;

        processedSamples += n; // 다음 호출을 위해 누적

        // 재생 시작 전 프리롤 구간은 분석 스킵
        if (chunkCenterDsp < startDspTime) return;

        // 평균 절댓값(모든 채널 평균)을 계산
        double sumAbs = 0.0;
        for (int i = 0; i < n; i++)
        {
            double s = 0.0;
            int baseIdx = i * channels;
            for (int c = 0; c < channels; c++)
                s += data[baseIdx + c];
            s /= channels; // 채널 평균
            sumAbs += Math.Abs(s);
        }
        double meanAbs = sumAbs / n;

        // 적응형 임계값: EMA * K
        ema = (1.0 - emaAlpha) * ema + emaAlpha * meanAbs;
        double threshold = ema * thresholdK;

        // 스파이크 감지: 임계초과 + (오디오 스레드 측 단순 디바운스)
        bool spike = meanAbs > threshold;
        if (spike)
        {
            lock (eventLock)
            {
                // 메인 스레드에서 최종 쿨다운을 적용하므로 여기서는 마지막 감지 시각만 업데이트
                lastDetectionDsp = chunkCenterDsp;
            }
        }
    }

    IEnumerator FadeOutVolume(float duration)
    {
        float t = 0f;
        float startVol = source.volume;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime; // 정지/느려짐에 영향받지 않게
            float k = Mathf.Clamp01(t / duration);
            source.volume = Mathf.Lerp(startVol, 0f, k);
            yield return null;
        }
        source.volume = 0f;
    }
}
