using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed, sensitivity;
    private float horizontalInput, verticalInput, mouseXInput, mouseYInput;

    void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(moveSpeed * Time.deltaTime * new Vector3(horizontalInput, 0, verticalInput));
    }

    void Rotate()
    {
        mouseXInput = Input.GetAxis("Mouse X");
        mouseYInput = Input.GetAxis("Mouse Y");
        transform.Rotate(sensitivity * Time.deltaTime * new Vector3(mouseYInput, mouseXInput, 0));
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
    }
}
