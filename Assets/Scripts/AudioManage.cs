using UnityEngine;

public class AudioManage : MonoBehaviour
{
    public AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource.volume = 0.0f;
        
    }
    float time = 0f;
    // Update is called once per frame
    void Update()
    {
        if (audioSource.volume < 1)
        {
            audioSource.volume += 0.01f;
        }
        if(time > 3f)
        {
            audioSource.Play();
        }
        time += Time.deltaTime;
    }
}
