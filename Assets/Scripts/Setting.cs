using UnityEngine;

public class Setting : MonoBehaviour
{
    private bool IsStopped = false;
    public GameObject panel;

    private void Start()
    {
        panel.SetActive(false);
    }

    public void TheWorld()
    {
        Time.timeScale = 0;
        panel.SetActive(IsStopped);
        AudioListener.pause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
    }
    public void Resume()
    {
        Time.timeScale = 1;
        panel.SetActive(IsStopped);
        AudioListener.pause = false;
        Cursor.lockState= CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���ø����̼� ����
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
