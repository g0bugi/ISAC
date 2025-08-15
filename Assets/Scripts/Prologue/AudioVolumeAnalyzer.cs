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
    private bool IsDestroy = false;
    public Sprite specificRippleSprite;
    public SoundLight rippleManager;
    GameObject BallParent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
                rippleManager.PlayRippleEffect(transform.position, source.maxDistance, specificRippleSprite);
                IsSpawned =true;
            }
        }
        timer += Time.deltaTime;
        time += Time.deltaTime;
    }

   
   
}
