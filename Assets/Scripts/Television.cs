using UnityEngine;
using UnityEngine.Video;

public class Television : MonoBehaviour, IInteractiable
{
    public VideoPlayer player;
    bool IsPlay = false;
   public void Action()
    {
        Debug.Log("Æ¼ºñ´Ù~~");
       IsPlay = !IsPlay;
        if (IsPlay)
            TurnOn();
        else
            TurnOff();
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
