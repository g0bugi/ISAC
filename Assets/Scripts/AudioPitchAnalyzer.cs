using System.Collections;
using UnityEngine;

public class AudioPitchAnalyzer : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject circlePrefab; // ������ ������ ����
    private float[] spectrum = new float[512];
    private bool hasSpawned = false; // �ߺ� ���� ����
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
       

        // 600~800Hz�� ���� �� �� �� ����
        if (freq > 500f && freq < 800f && !hasSpawned)
        {
            SpawnCircle();
            hasSpawned = true;
        }

        // ���ļ��� �ٽ� �������� ���� ���� �����ϵ��� �÷��� �ʱ�ȭ
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
