using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    CharacterController cc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }
    public float moveSpeed = 7.0f;
   
    float gravity = -20f;
    float yVelocity = 0f;
    public float jumpPower = 10f;
    public bool isJumping = false;
    // Update is called once per frame
    void Update()
    {
        if(cc.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0f;
            isJumping = false;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        dir = Camera.main.transform.TransformDirection(dir);

        if(Input.GetButton("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }

        yVelocity += gravity*Time.deltaTime;
        dir.y = yVelocity;

        cc.Move(dir * moveSpeed * Time.deltaTime);
    }
}
