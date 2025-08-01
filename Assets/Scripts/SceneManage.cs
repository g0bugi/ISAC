using UnityEditor.Build.Content;
using UnityEngine;

public class SceneManage : MonoBehaviour
{
    public static SceneManage Instance;

    public int entryPointID = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        switch (SceneManage.Instance.entryPointID)
        {
            case 0:
                transform.position = new Vector3(2, 1, -9);
                break;
            case 1:
                transform.position = new Vector3(17.45f, 1, 0);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
