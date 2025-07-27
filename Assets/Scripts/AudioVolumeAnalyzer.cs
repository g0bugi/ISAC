using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioVolumeAnalyzer : MonoBehaviour
{
    public AudioSource source;
    AudioClip clip;
    public AudioDistortionFilter distortionFilter;
    float[] spectrum = new float[512];
    public GameObject circlePrefab;
    private bool IsSpawned = false;
    public float Waittime = 6;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clip = source.clip;
        StartCoroutine(Play());
        source.volume = 1;
    }
    IEnumerator Play()
    {
        yield return new WaitForSeconds(Waittime);
        source.Play();
        yield break;
    }
    int cycle = 0;
    float timer = 0f;
    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying) return;
        if(timer> 0.05)
        {
            IsSpawned = false;
        }
        
        clip.GetData(spectrum, cycle * 512);
        for (int i = 0; i < spectrum.Length; i++)
        {
            if (spectrum[i] > 0.1 && !IsSpawned)
            {
                SpawnCircle(spectrum[i]);
                IsSpawned =true;
            }
        }
        cycle++;
        timer += Time.deltaTime;
    }
    void SpawnCircle(float volume)
    {
        Vector3 spawnPos = new Vector3(Random.Range(-16f, 16f), Random.Range(-9f, 9f), 0f);
        GameObject colorball = Instantiate(circlePrefab, spawnPos, Quaternion.identity);
        colorball.transform.localScale *= 10f * volume;
        if (distortionFilter.distortionLevel < 0.98)
        {
            
            distortionFilter.distortionLevel += 0.005f;
        }
    }
}
