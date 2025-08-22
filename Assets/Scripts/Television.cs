using UnityEngine;
using UnityEngine.Video;

public class Television : MonoBehaviour, IInteractiable
{
    public VideoPlayer player;
    public GameObject sound;
    bool IsPlay = false;
    public GameObject noise;
    
    void Start()
    {
        noise.SetActive(false);
    }
   public void Action()
    {
        Debug.Log("Ƽ���~~");
       IsPlay = !IsPlay;
        if (IsPlay)
        {
            TurnOn();
            noise.SetActive(true);
        }
        else
        {
            TurnOff();
            noise.SetActive(false);
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
