using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Tarnslational Settings")]
    [Range(0, 100)]
    public float moveSpeed;

    [Header("Rotation Settings")]
    [Range(100, 1000)]
    public float rotateSpeed;
    [Range(-45, -30)]
    public float minAngle;
    [Range(30, 75)]
    public float maxAngle;

    [Header("References")]
    private float horizontalInput, verticalInput;
    private float mouseXInput, mouseYInput;

    void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Time.deltaTime * moveSpeed * new Vector3(horizontalInput, 0, verticalInput));
    }

    void Rotate()
    {
        mouseXInput = Input.GetAxis("Mouse X");
        mouseYInput = Input.GetAxis("Mouse Y");
        transform.Rotate(Time.deltaTime * rotateSpeed * mouseXInput * Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
    }
}
