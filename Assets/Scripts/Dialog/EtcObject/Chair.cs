using UnityEngine;

public class Chair : MonoBehaviour, IInteractiable
{
    public GameObject player;
    public void Action()
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.MovePosition(transform.position + Vector3.up * 0.5f);
        }
        else
        {
            player.transform.position = transform.position + Vector3.up * 0.5f;
        }

        Debug.Log("check: moved player");
    }
}
