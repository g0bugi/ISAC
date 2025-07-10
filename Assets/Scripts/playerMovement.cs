using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float moveSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.position += moveSpeed * Time.deltaTime * Vector3.left;
        if (Input.GetKey(KeyCode.RightArrow))
            transform.position += moveSpeed * Time.deltaTime * Vector3.right;
        if (Input.GetKey(KeyCode.UpArrow))
            transform.position += moveSpeed * Time.deltaTime * Vector3.forward;
        if (Input.GetKey(KeyCode.DownArrow))
            transform.position += moveSpeed * Time.deltaTime * Vector3.back;
    }
}
