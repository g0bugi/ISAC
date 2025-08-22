using UnityEngine;
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{
    private bool IsStopped = false;
    public GameObject setting;
    public Canvas can;
    
    void Awake()
    {
        DontDestroyOnLoad(can);
       
    }

    private void Start()
    {
        setting.SetActive(false);
    }

    public void TheWorld()
    {
        Time.timeScale = 0;
        setting.SetActive(IsStopped);
        AudioListener.pause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
    }
    public void Resume()
    {
        Time.timeScale = 1;
        setting.SetActive(IsStopped);
        AudioListener.pause = false;
        Cursor.lockState= CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
        Debug.Log("");
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
