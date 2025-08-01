using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(MeshFilter))]
public class LC_Door : MonoBehaviour
{
    public enum rotOrient
    {
        Y_Axis_Up,
        Z_Axis_Up,
        X_Axis_Up
    }

    public enum rotFixAxis
    {
        Y,
        Z
    }

    public rotOrient rotationOrientation;
    public bool applyRotationFix = false;
    public rotFixAxis rotationAxisFix;
    public float doorOpenAngle = 90.0f;
    [Range(1, 15)] public float speed = 10.0f;

    public Transform hingePoint;
    public float hingeOffset = 0.5f;

    Quaternion doorOpen = Quaternion.identity;
    Quaternion doorClosed = Quaternion.identity;
    Vector3 doorClosedPos;
    Vector3 doorOpenPos;
    bool doorStatus = false;

    void Start()
    {
        if (this.gameObject.isStatic)
        {
            Debug.Log("This door has been set to static and won't be openable. Doorscript has been removed.");
            Destroy(this);
            return;
        }

        if (hingePoint == null)
        {
            Vector3 hingePos = transform.position + transform.right * -hingeOffset;
            GameObject hinge = new GameObject("Door_Hinge");
            hinge.transform.position = hingePos;
            hinge.transform.rotation = transform.rotation;
            hingePoint = hinge.transform;
        }

        doorClosed = transform.rotation;
        doorClosedPos = transform.position;

        CalculateDoorOpenTransform();
    }

    void CalculateDoorOpenTransform()
    {
        Vector3 hingeToDoor = transform.position - hingePoint.position;

        Quaternion rotation = Quaternion.identity;
        switch (rotationOrientation)
        {
            case rotOrient.Y_Axis_Up:
                rotation = Quaternion.AngleAxis(doorOpenAngle, Vector3.up);
                break;
            case rotOrient.Z_Axis_Up:
                rotation = Quaternion.AngleAxis(doorOpenAngle, Vector3.forward);
                break;
            case rotOrient.X_Axis_Up:
                rotation = Quaternion.AngleAxis(doorOpenAngle, Vector3.right);
                break;
        }

        Vector3 rotatedOffset = rotation * hingeToDoor;
        doorOpenPos = hingePoint.position + rotatedOffset;

        doorOpen = doorClosed * rotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!doorStatus)
            {
                InteractWithThisDoor();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (doorStatus)
            {
                InteractWithThisDoor();
            }
        }
    }

    public void InteractWithThisDoor()
    {
        if (doorStatus)
        {
            StartCoroutine(this.moveDoor(false));
        }
        else
        {
            StartCoroutine(this.moveDoor(true));
        }
    }

    IEnumerator moveDoor(bool opening)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = opening ? doorOpenPos : doorClosedPos;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = opening ? doorOpen : doorClosed;

        float elapsedTime = 0f;
        float totalTime = 1f / speed;

        while (elapsedTime < totalTime)
        {
            float t = elapsedTime / totalTime;
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;

        doorStatus = !doorStatus;
        yield return null;
    }
}