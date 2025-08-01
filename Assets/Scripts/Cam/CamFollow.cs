using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [Header("References")]
    public Transform target;

    void Update()
    {
        transform.position = target.position;
    }
}
