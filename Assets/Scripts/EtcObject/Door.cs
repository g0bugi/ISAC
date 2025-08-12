using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractiable
{
    public bool isOpen = false;
    public float angle = 90f;     // 열리는 각도
    public float duration = 1f;   // 여닫는 시간

    private float currentAngle = 0f;
    private Vector3 originPosition;
    private Quaternion originRotation;
    private Coroutine routine;

    void Start()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
    }

    public void Action()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(RotateAndPush());
    }

    IEnumerator RotateAndPush()
    {
        float startAngle = currentAngle;
        float endAngle = isOpen ? 0f : -angle; // 바깥쪽으로 여는 각도

        float time = 0f;
        float k = transform.localScale.x / 2f; // 회전 중심으로부터 거리

        while (time < duration)
        {
            float t = time / duration;
            float a = Mathf.Lerp(startAngle, endAngle, t) * Mathf.Deg2Rad;

            // 회전
            transform.rotation = Quaternion.Euler(0, Mathf.Lerp(startAngle, endAngle, t), 0);

            // 위치 보정 (축은 오른쪽 기준이라고 가정, 보정 방향도 반대로)
            float xOffset = -k * (1 - Mathf.Cos(a)); // X축 반대 방향으로 당김
            float zOffset = -k * Mathf.Sin(a);        // Z축 양의 방향으로 밀림

            transform.position = originPosition + new Vector3(xOffset, 0, zOffset);

            time += Time.deltaTime;
            yield return null;
        }

        // 최종 위치 정리
        float finalA = endAngle * Mathf.Deg2Rad;
        float finalX = -k * (1 - Mathf.Cos(finalA));
        float finalZ = -k * Mathf.Sin(finalA);

        transform.rotation = Quaternion.Euler(0, endAngle, 0);
        transform.position = originPosition + new Vector3(finalX, 0, finalZ);

        currentAngle = endAngle;
        isOpen = !isOpen;
    }
}
