using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioVolumeAnalyzer : MonoBehaviour
{
    public AudioSource source;
    AudioClip clip;
    public AudioDistortionFilter distortionFilter;
    float[] spectrum = new float[512];
    public GameObject circlePrefab;
    private bool IsSpawned = false;
    public float Waittime = 8;
    public GameObject Parent_1;
    public GameObject Parent_2;
    private bool IsDestroy = false;
    GameObject BallParent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        BallParent = Parent_1;
        clip = source.clip;
        StartCoroutine(Play());
        source.volume = 10;
    }

    IEnumerator Play()
    {
        yield return new WaitForSeconds(Waittime);
        source.Play();
        yield break;
    }

    int cycle = 0;
    float timer = 0f;
    float time = 0f;

    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying) return;
        if (time > 60f)
        {
            SceneManage.Instance.entryPointID = 0;
            SceneManager.LoadScene("Hospital_1F");
        }
        if(timer> 0.1)
        {
            IsSpawned = false;
            timer = 0f;
        }

        if(time > 50f)
        {
            source.volume -= 0.005f;
        }

        if (time > 37f && !IsDestroy)
        {
            BallParent = Parent_2;
            Destroy(Parent_1);
            IsDestroy = true;
            
        }
        clip.GetData(spectrum, cycle * 512);
        for (int i = 0; i < spectrum.Length; i++)
        {
            if (spectrum[i] > 0.05 && !IsSpawned)
            {
                SpawnCircle(spectrum[i]);
                IsSpawned =true;
            }
        }
        cycle++;
        timer += Time.deltaTime;
        time += Time.deltaTime;
    }

    void SpawnCircle(float volume)
    {
        Vector3 spawnPos = new Vector3(Random.Range(-16f, 16f), Random.Range(-9f, 9f), 0f);
        GameObject colorball = Instantiate(circlePrefab, spawnPos, Quaternion.identity, BallParent.transform);
        colorball.transform.localScale *= 10f * volume;
        if (distortionFilter.distortionLevel < 0.98)
        {
            
            distortionFilter.distortionLevel += 0.005f;
            
        }
    }
   
}
