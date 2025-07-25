using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioVolumeAnalyzer : MonoBehaviour
{
    public AudioSource source;
    AudioClip clip;
    float[] spectrum = new float[512];
    public GameObject circlePrefab;
    private bool IsSpawned = false;
    public bool noisy = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clip = source.clip;
        StartCoroutine(Play());
    }
    IEnumerator Play()
    {
        yield return new WaitForSeconds(3);
        source.Play();
        yield break;
    }
    int cycle = 0;
    float timer = 0;
    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying) return;
        if(noisy)
        {

        }
            IsSpawned = false;
        clip.GetData(spectrum, cycle*512);
        for (int i = 0; i < spectrum.Length; i++)
        {
            if (spectrum[i] > 0.4 && !IsSpawned)
            {
                SpawnCircle();
                IsSpawned =true;
            }
        }
        cycle++;
        timer += Time.deltaTime;
    }
    void SpawnCircle()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 0f);
        Instantiate(circlePrefab, spawnPos, Quaternion.identity);
    }
}
