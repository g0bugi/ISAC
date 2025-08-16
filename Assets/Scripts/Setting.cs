using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Setting : MonoBehaviour
{
    private bool IsStopped = false;
    public Image panel;
    private void Start()
    {
        panel.enabled = false;
    }
    
    public void TheWorld()
    {
        Time.timeScale = 0;
        panel.enabled = true;
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios)
        {
            if (audio.isPlaying)
                audio.Pause(); // 재생 위치 기억, Resume 가능
        }
        Debug.Log("인식 완료");
    }
    public void Resume()
    {
        Time.timeScale = 1;
        panel.enabled = false;
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios)
        {
            if (!audio.isPlaying)
                audio.UnPause(); // 재생 위치 기억, Resume 가능
        }
    }
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsStopped = !IsStopped;
            if (IsStopped)
            {
                TheWorld();
            }
            if (!IsStopped)
            {
                Resume();
            }
        }
        
    }

   

}
