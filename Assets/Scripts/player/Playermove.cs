using UnityEngine;
using System.Collections; // IEnumerator를 위해 필요

public class Playermove : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 8f; // 점프 높이 조절
    public float gravity = -9.81f; // 중력 값 (유니티 기본 중력)

    public float mouseSensitivity = 2f;
    public Transform cameraTransform;

    private CharacterController controller; 
    private Vector3 playerVelocity; //플레이어의 현재 속도 (중력, 점프 등)
    private bool isGrounded; // 땅에 닿아있는지 여부
    private float cameraPitch = 0f;

    public Animator playerAnimator;
    public string getUpAnimationTrigger = "GetUp";
    public string speedParameterName = "Speed"; 
    public string jumpParameterName = "Jump"; 

    public bool canMove = true; // 플레이어 이동 가능 여부 플래그

    public DialogManager manager; // 대화 관리자
    void Awake()
    {
        
        // CharacterController 컴포넌트 가져오기
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController가 playermove 스크립트에 할당되지 않았습니다!");
        }

        if (cameraTransform == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                cameraTransform = mainCam.transform;
            }
            else
            {
                Debug.LogWarning("Main Camera를 찾을 수 없습니다. cameraTransform을 수동으로 할당해주세요.");
            }
        }

        if (playerAnimator == null)
        {
            playerAnimator = GetComponent<Animator>();
            if (playerAnimator == null)
            {
                Debug.LogWarning("Animator 컴포넌트가 Player GameObject에 없습니다. 수동으로 할당해주세요.");
            }
        }

        canMove = false;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (manager.isAction)
            canMove = false;
        else
            canMove = true;
        if (!canMove) return;

        //Debug.Log($"[Update] → 이동 처리 시작");

        isGrounded = controller.isGrounded;

        // 중력 적용
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2.0f;
        }

        RotateView();
        HandleJump(); 
    }

    void FixedUpdate() 
    {
        if (!canMove) return;
        //Debug.Log("[FixedUpdate] 이동 처리 진입");
        HandleMovement(); 
    }

    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        
        Vector3 moveDirection = transform.right * h + transform.forward * v;

        // 정규화 (대각선 이동 속도 보정)
        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        if (playerAnimator != null)
        {
            float animationSpeed = new Vector3(moveDirection.x, 0, moveDirection.z).magnitude * currentSpeed;
            playerAnimator.SetFloat(speedParameterName, animationSpeed);
        }
    }

    void HandleJump() // 점프 처리 함수 분리 및 CharacterController에 맞게 수정
    {
        //Debug.Log($"isGrounded: {isGrounded}, playerVelocity.y: {playerVelocity.y:F2}, Spacebar Pressed: {Input.GetKeyDown(KeyCode.Space)}"); 

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);

        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void RotateView()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f); 
        cameraTransform.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);
    }

    public void EnableMovement()
    {
        this.enabled = true;
        canMove = true;
        Debug.Log("EnableMovement 플레이어 이동 활성화.");


        if (controller != null)
        {
            Debug.Log($"CharacterController 활성 상태: {controller.enabled}");
        }

        if (cameraTransform == null)
        {
            Debug.LogWarning("카메라가 없습니다!");
        }
        // 이동이 활성화될 때 마우스 커서도 다시 잠급니다. (이미 Start에서 했지만 혹시 모를 경우)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetCanMove(bool state)
    {
        canMove = state;

        if (!state && playerAnimator != null)
        {
            playerAnimator.SetFloat(speedParameterName, 0f);
        }

        
        if (state) 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else 
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        Debug.Log($"플레이어 이동 {(state ? "활성화" : "비활성화")}.");
    }

    public void StartGettingUpAnimation()
    {
        canMove = false;
        Debug.Log("StartGettingUpAnimation 플레이어 이동 비활성화.");

        if (playerAnimator != null && !string.IsNullOrEmpty(getUpAnimationTrigger))
        {
            playerAnimator.SetTrigger(getUpAnimationTrigger);
            Debug.Log($"일어나는 애니메이션 '{getUpAnimationTrigger}' 트리거됨.");

            // 일어나는 애니메이션이 끝난 후 이동 활성화.
            // 애니메이션 이벤트(Animation Event)를 사용하는 것이 가장 좋지만,
            // 임시로 Coroutine을 사용하여 애니메이션 길이만큼 기다리게 할 수 있습니다.
            StartCoroutine(DelayEnableMovementAfterAnimation());
        }
        else
        {
            Debug.LogWarning("Animator 또는 GetUp 애니메이션 트리거가 설정되지 않았습니다.");
            // 애니메이션이 없거나 설정되지 않았다면 바로 이동 활성화
            EnableMovement();
        }
    }

    IEnumerator DelayEnableMovementAfterAnimation()
    {
        // 일어나는 애니메이션 클립의 정확한 길이를 아는 것이 가장 좋습니다.
        // 또는 애니메이션 이벤트 (Animation Event)를 사용하세요.
        // 예시: yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(5.0f); // 임의의 대기 시간 (애니메이션 길이에 맞게 조절)

        EnableMovement();
        
    }

}