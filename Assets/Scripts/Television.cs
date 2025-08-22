using UnityEngine;
using UnityEngine.Video;

public class Television : MonoBehaviour, IInteractiable
{
    public VideoPlayer player;
    public GameObject sound;
    bool IsPlay = false;
    
    void Start()
    {
        sound.SetActive(false);
    }
   public void Action()
    {
        Debug.Log("Æ¼ºñ´Ù~~");
       IsPlay = !IsPlay;
        if (IsPlay)
        {
            TurnOn();
            sound.SetActive(true);
        }
        else
        {
            TurnOff();
            sound.SetActive(false);
        }
    }

    private void TurnOn()
    {
        player.Play();
    }
    private void TurnOff()
    {
        player.Stop();
    }
}
