using UnityEngine;
using System.Collections;
public class CamRotate : MonoBehaviour
{
    public float rotateSpeed = 1.0f;
    float mx = 0;
    float my = 0;

    IEnumerator Waitfor()
    {
        yield return new WaitForSeconds(5);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Waitfor());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        mx += mouse_X * rotateSpeed * Time.deltaTime;
        my += mouse_Y * rotateSpeed * Time.deltaTime;

        my = Mathf.Clamp(my, -12f, 10f);
        mx = Mathf.Clamp(mx, -10f, 10f);
        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
