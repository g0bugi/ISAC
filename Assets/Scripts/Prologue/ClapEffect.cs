using UnityEngine;

public class ClapEffect : MonoBehaviour
{
    public AudioSource Clap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Clap.Play();
    }
   
}
