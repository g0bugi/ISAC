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
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Waitfor());
       
    }

    // Update is called once per frame
    void Update()
    {
        

        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        mx += mouse_X * rotateSpeed * Time.deltaTime;
        my += mouse_Y * rotateSpeed * Time.deltaTime;

        my = Mathf.Clamp(my, -12f, 10f);
        mx = Mathf.Clamp(mx, -10f, 10f);
        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
