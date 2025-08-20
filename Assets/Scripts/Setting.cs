using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Setting : MonoBehaviour
{
    private bool IsStopped = false;
    public GameObject panel;
    public SoundLight SoundLight;
   
    private void Start()
    {
        panel.SetActive(false);
    }
    
    public void TheWorld()
    {
        Time.timeScale = 0;
        panel.SetActive(IsStopped);
        AudioListener.pause = true;
        /*AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios)
        {
            if (audio.isPlaying)
                audio.Pause(); // 재생 위치 기억, Resume 가능
        }*/
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("인식 완료");
    }
    public void Resume()
    {
        Time.timeScale = 1;
        panel.SetActive(IsStopped);
        AudioListener.pause = false;
        
        /*AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios)
        {
            if (!audio.isPlaying)
                audio.UnPause(); // 재생 위치 기억, Resume 가능
        }*/
        Cursor.lockState= CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
        Debug.Log("종료~~");
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
