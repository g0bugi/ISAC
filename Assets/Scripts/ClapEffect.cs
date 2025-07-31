using UnityEngine;

public class ClapEffect : MonoBehaviour
{
    public AudioSource Clap;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Clap.Play();
    }
    float time = 0.0f;
    // Update is called once per frame
    void Update()
    {
        if(time > 5f)
        {
            Clap.Stop();
            return;
        }
        time += Time.deltaTime;
    }
}
