using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Tarnslational Settings")]
    [Range(0, 100)]
    public float moveSpeed;
    const float gravity = 9.8f;
    public Vector3 playerDirection;

    [Header("Rotation Settings")]
    [Range(100, 1000)]
    public float rotateSpeed;
    [Range(-45, -30)]
    public float minAngle;
    [Range(30, 75)]
    public float maxAngle;

    [Header("References")]
    public GameObject playerCamera;

    void Move()
    {
        playerDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // if (!playerController.isGrounded) { playerDirection.y -= gravity * Time.deltaTime; }
        transform.Translate(moveSpeed * Time.deltaTime * playerDirection);
    }

    void Rotate()
    {
        transform.Rotate(rotateSpeed * Input.GetAxis("Mouse X") * Time.deltaTime * Vector3.up);
        playerCamera.transform.Rotate(rotateSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime * Vector3.left);
    }

    void StandUp()
    {
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
        if (Input.GetKeyDown(KeyCode.R)) { StandUp(); }
    }
}
