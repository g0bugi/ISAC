using UnityEngine;
using UnityEngine.Video;

public class Television : MonoBehaviour, IInteractiable
{
    public VideoPlayer player;
    bool IsPlay = false;
    public AudioSource noise;
   public void Action()
    {
        Debug.Log("Æ¼ºñ´Ù~~");
       IsPlay = !IsPlay;
        if (IsPlay)
        {
            TurnOn();
            noise.Play();
        }
        else
        {
            TurnOff();
            noise.Stop();
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
