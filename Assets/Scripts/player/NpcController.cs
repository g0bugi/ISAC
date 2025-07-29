using UnityEngine;
using System.Collections; // Coroutine을 위해 필요

public class NpcController : MonoBehaviour
{
    public Animator animator; // NPC의 Animator 컴포넌트 (Inspector에서 연결)
    public float walkSpeed = 2.0f; // NPC의 걷는 속도
    public float rotationSpeed = 5.0f; // NPC의 회전 속도 (이동 시)

    private Vector3 currentTargetPosition;
    private bool isMoving = false;

    void Awake()
    {
        // Animator 컴포넌트가 연결되지 않았다면, 같은 오브젝트에서 찾기 시도
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    // NPC를 앉히거나 서게 하는 함수
    public void SetSitting(bool isSitting)
    {
        if (animator != null)
        {
            // Animator에 "IsSitting"이라는 bool 파라미터가 있다고 가정
            animator.SetBool("IsSitting", isSitting);
            // 앉을 때는 걷는 애니메이션을 끄고, 일어설 때는 걷는 애니메이션을 끔 (혹시 모를 충돌 방지)
            animator.SetBool("IsWalking", false);
        }
    }

    // NPC가 특정 위치로 걷기 시작하는 함수
    public void StartWalkingTo(Vector3 targetPosition)
    {
        currentTargetPosition = targetPosition;
        isMoving = true;
        if (animator != null)
        {
            // Animator에 "IsWalking"이라는 bool 파라미터가 있다고 가정
            animator.SetBool("IsWalking", true);
            // 앉기 애니메이션을 끔 (앉아있다가 걸어야 하므로)
            animator.SetBool("IsSitting", false);
        }
        StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget()
    {
        while (isMoving && Vector3.Distance(transform.position, currentTargetPosition) > 0.1f)
        {
            // 목표 방향으로 회전
            Vector3 direction = (currentTargetPosition - transform.position).normalized;
            if (direction != Vector3.zero) // 방향이 0 벡터가 아닐 때만 회전
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // 목표 위치로 이동
            transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, walkSpeed * Time.deltaTime);
            yield return null;
        }

        // 목표 위치에 도달하면 걷는 애니메이션 끄기
        isMoving = false;
        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
        }
        Debug.Log("NPC 이동 완료!");
    }
}