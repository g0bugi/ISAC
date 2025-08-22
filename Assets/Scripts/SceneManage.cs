using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public static SceneManage Instance;
    public GameObject player;

    public int entryPointID = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the event
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This method is called every time a scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the player in the newly loaded scene
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            switch (entryPointID)
            {
                case 0:
                    player.transform.position = new Vector3(1.75f, 1, -10);
                    break;
                case 1:
                    player.transform.position = new Vector3(13, 0.2f, 2);
                    player.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                    break;
                default:
                    // You can add a default behavior or a log message here if you want
                    break;
            }
        }
        else
        {
            Debug.LogWarning("SceneManage: Player object with 'Player' tag not found in scene " + scene.name);
        }
    }
}
