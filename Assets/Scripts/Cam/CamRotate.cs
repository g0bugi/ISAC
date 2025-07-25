 using UnityEngine;
using System.Collections;
public class CamRotate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Waitfor());
    }
    IEnumerator Waitfor()
    {
        yield return new WaitForSeconds(5);
    }
    public float rotspeed = 1.0f;
    float mx = 0;
    float my = 0;
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

        mx += mouse_X * rotspeed * Time.deltaTime;
        my += mouse_Y * rotspeed * Time.deltaTime;

        my = Mathf.Clamp(my, -20f, 20f);
        mx = Mathf.Clamp(mx, -30f, 30f);
        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
