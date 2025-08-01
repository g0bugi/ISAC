using UnityEngine;
using UnityEngine.AI;
using System.Collections; // Coroutine을 위해 필요
using System;

public class NpcController : MonoBehaviour
{
    public Animator animator; // NPC의 Animator 컴포넌트 (Inspector에서 연결)
    public float walkSpeed = 2.0f; // NPC의 걷는 속도
    public float rotationSpeed = 5.0f; // NPC의 회전 속도 (이동 시)

    private Vector3 currentTargetPosition;
    private Vector3 initialDialoguePosition; // 대화 시작 시 NPC의 원래 위치
    private Quaternion initialDialogueRotation; // 원래 회전값
    private Action onMovementCompleteCallback; // 이동 완료 시 호출할 콜백
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
            initialDialoguePosition = transform.position;
            initialDialogueRotation = transform.rotation;
        }
    }

    public void MoveToPosition(Vector3 targetPos, float speed, Action callback = null, bool returnToInitial = false)
    {
        if (isMoving) return; // 이미 이동 중이면 무시

        isMoving = true;
        onMovementCompleteCallback = callback; // 이동 완료 시 호출할 콜백 저장
        StartCoroutine(MoveNpcCoroutine(targetPos, speed, returnToInitial));
    }

    private IEnumerator MoveNpcCoroutine(Vector3 targetPos, float speed, bool returnToInitial)
    {
        Vector3 currentTarget = targetPos; // 현재 이동할 목표

        if (animator != null)
        {
            // Animator에 "IsWalking"이라는 bool 파라미터가 있다고 가정
            animator.SetBool("IsWalking", true);
            // 앉기 애니메이션을 끔 (앉아있다가 걸어야 하므로)
            animator.SetBool("IsSitting", false);
        }

        // 첫 번째 이동: 목표 위치로
        while (Vector3.Distance(transform.position, currentTarget) > 0.1f) // 충분히 가까워질 때까지 이동
        {
            Vector3 directionToTarget = (currentTarget - transform.position).normalized;
            if (directionToTarget != Vector3.zero) // 0 벡터가 아닐 때만 회전
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed * 10f); // 5f는 회전 속도 조절
            }
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = currentTarget; // 정확한 위치로 설정

        // 만약 돌아와야 한다면 두 번째 이동: 원래 위치로
        if (returnToInitial)
        {
            yield return new WaitForSeconds(1.0f); // 잠시 기다림 (선택 사항)
            currentTarget = initialDialoguePosition; // 원래 위치로 목표 변경

            while (Vector3.Distance(transform.position, currentTarget) > 0.1f) // 충분히 가까워질 때까지 이동
        {
            Vector3 directionToTarget = (currentTarget - transform.position).normalized;
            if (directionToTarget != Vector3.zero) // 0 벡터가 아닐 때만 회전
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed * 10f); // 5f는 회전 속도 조절
            }
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
            yield return null;
        }
            transform.position = currentTarget; // 정확한 위치로 설정
            
            float currentRotationTime = 0f;
            float totalRotationTime = 0.5f; // 원래 방향으로 돌아오는 데 걸릴 시간
            Quaternion startRotation = transform.rotation;

            while (currentRotationTime < totalRotationTime)
            {
                transform.rotation = Quaternion.Slerp(startRotation, initialDialogueRotation, currentRotationTime / totalRotationTime);
                currentRotationTime += Time.deltaTime;
                yield return null;
            }
            transform.rotation = initialDialogueRotation; // 정확히 원래 회전값으로 설정
        }

        Debug.Log("NPC 이동 완료!");
        isMoving = false; // 이동 상태 해제
        onMovementCompleteCallback?.Invoke(); // 이동 완료 콜백 호출
        onMovementCompleteCallback = null; // 콜백 초기화
        SetSitting(true);
    }

    public bool IsMoving => isMoving;


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

    public bool GetIsMoving()
    {
        return isMoving;
    }
}