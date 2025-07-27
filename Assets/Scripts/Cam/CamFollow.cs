using UnityEngine;

public class CamFollow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    //카메라가 들어갈 타겟
    public Transform target;
    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;
    }
}
