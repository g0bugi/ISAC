using UnityEngine;

public class Chair : MonoBehaviour, IInteractiable
{
    public GameObject player;
    public void Action()
    {
        player.transform.position = transform.position;
        Debug.Log("check");
    }
}
