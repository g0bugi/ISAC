using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class AudioVolumeAnalyzer : MonoBehaviour
{
    public AudioSource source;
    AudioClip clip;
    public AudioDistortionFilter distortionFilter;
    float[] spectrum = new float[512];
    public GameObject circlePrefab;
    private bool IsSpawned = false;
    public float Waittime = 8;
    public Sprite specificRippleSprite;
    public SoundLight rippleManager;
    GameObject BallParent;
    private bool TimeToSilence = false;
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
            StartSilence();
        }

       
        int offset = cycle * 512;
        if (clip != null && clip.loadState == AudioDataLoadState.Loaded && offset + 512 <= clip.samples)
        {
            clip.GetData(spectrum, offset);
            cycle++;
        }

        for (int i = 0; i < spectrum.Length; i++)
        {
            if (spectrum[i] > 0.05 && !IsSpawned)
            {
                Vector3 Randir = new Vector3 (Random.Range(-10f, 10f), Random.Range(-10f,10f),Random.Range(-10f,10f));
                rippleManager.PlayRippleEffect(transform.position + Randir , source.maxDistance, specificRippleSprite);
                IsSpawned =true;
            }
        }
        timer += Time.deltaTime;
        time += Time.deltaTime;
    }
    void StartSilence()
    {
        if(TimeToSilence)
        {
            StartCoroutine(Silence());
            TimeToSilence = false;
        }
        else
        {
            return;
        }
    }

    IEnumerator Silence()
    {
        float time = 0f;
        float active = 10f;
        while (time < active)
        {
            source.volume = Mathf.Lerp(1f, 0f, time / active);
            time += Time.deltaTime;

            yield return null;
        }
        yield break;
    }
   
}
