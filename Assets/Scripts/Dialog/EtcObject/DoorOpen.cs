using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class DoorOpen : MonoBehaviour
{
    public enum RotateOrient
    {
        Y_Axis_Up,
        Z_Axis_Up,
        X_Axis_Up
    }

    public RotateOrient rotateOrientation;
    public bool applyRotationFix = false;
    public float doorOpenAngle = -85f;
    [Range(1, 15)] public float speed = 5f;

    public Transform hingePoint;
    public float hingeOffset = 0.5f;

    Vector3 openPosition;
    Vector3 closedPosition;
    Quaternion openAngle = Quaternion.identity;
    Quaternion closedAngle = Quaternion.identity;
    public bool isOpen = false;

    public Transform doorModelTransform;

    void CalculateDoorOpenTransform()
    {
        Vector3 hingeToDoor = doorModelTransform.position - hingePoint.position;
        Quaternion rotation = Quaternion.identity;
        switch (rotateOrientation)
        {
            case RotateOrient.Y_Axis_Up:
                rotation = Quaternion.AngleAxis(doorOpenAngle, Vector3.up);
                break;
            case RotateOrient.Z_Axis_Up:
                rotation = Quaternion.AngleAxis(doorOpenAngle, Vector3.forward);
                break;
            case RotateOrient.X_Axis_Up:
                rotation = Quaternion.AngleAxis(doorOpenAngle, Vector3.right);
                break;
        }

        Vector3 rotatedOffset = rotation * hingeToDoor;
        openPosition = hingePoint.position + rotatedOffset;

        openAngle = closedAngle * rotation;
    }

    IEnumerator MoveDoor(bool opening)
    {
        Vector3 startPosition = doorModelTransform.position;
        Vector3 targetPosition = opening ? openPosition : closedPosition;
        Quaternion startRotation = doorModelTransform.rotation;
        Quaternion targetRotation = opening ? openAngle : closedAngle;

        float elapsedTime = 0f;
        float totalTime = 1f / speed;

        while (elapsedTime < totalTime)
        {
            float t = elapsedTime / totalTime;
            t = Mathf.SmoothStep(0f, 1f, t);
            doorModelTransform.SetPositionAndRotation(
                Vector3.Lerp(startPosition, targetPosition, t),
                Quaternion.Slerp(startRotation, targetRotation, t)
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        doorModelTransform.SetPositionAndRotation(targetPosition, targetRotation);
        isOpen = !isOpen;
        yield return null;
    }

    public void InteractWithThisDoor()
    {
        if (isOpen) { StartCoroutine(MoveDoor(false)); }
        else { StartCoroutine(MoveDoor(true)); }
    }

    void Start()
    {
        if (gameObject.isStatic)
        {
            Debug.Log("This door has been set to static and won't be openable. Doorscript has been removed.");
            Destroy(this);
            return;
        }
        if (transform.childCount != 0) { doorModelTransform = transform.GetChild(0); }
        if (hingePoint == null)
        {
            GameObject hinge = new();
            hinge.transform.SetPositionAndRotation(
                transform.position + hingeOffset * transform.right,
                transform.rotation
            );
            hingePoint = hinge.transform;
        }

        closedAngle = doorModelTransform.rotation;
        closedPosition = doorModelTransform.position;

        CalculateDoorOpenTransform();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen) { InteractWithThisDoor(); }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isOpen) { InteractWithThisDoor(); }
    }
}