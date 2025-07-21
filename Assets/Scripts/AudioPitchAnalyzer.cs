using System.Collections;
using UnityEngine;

public class AudioPitchAnalyzer : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject circlePrefab; // 프리팹 연결할 슬롯
    private float[] spectrum = new float[512];
    private bool hasSpawned = false; // 중복 생성 방지
    void Start()
    {
        StartCoroutine(Play());
    }
    IEnumerator Play()
    {
        yield return new WaitForSeconds(3);
        audioSource.Play();
        yield break;
    }
    void Update()
    {
        if (!audioSource.isPlaying) return;

        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        int maxIndex = 0;
        float maxVal = 0f;
        for (int i = 0; i < spectrum.Length; i++)
        {
            if (spectrum[i] > maxVal)
            {
                maxVal = spectrum[i];
                maxIndex = i;
            }
        }

        float freq = maxIndex * AudioSettings.outputSampleRate / 2f / spectrum.Length;
       

        // 600~800Hz일 때만 한 번 원 생성
        if (freq > 500f && freq < 800f && !hasSpawned)
        {
            SpawnCircle();
            hasSpawned = true;
        }

        // 주파수가 다시 낮아지면 다음 생성 가능하도록 플래그 초기화
        if (freq < 200f)
        {
            hasSpawned = false;
        }
    }

    void SpawnCircle()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 0f);
        Instantiate(circlePrefab, spawnPos, Quaternion.identity);
    }
}
