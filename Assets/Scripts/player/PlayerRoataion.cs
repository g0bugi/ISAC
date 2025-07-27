using UnityEngine;

public class PlayerRoataion : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public float rotspeed = 10.0f;
    float mx = 0;
    // Update is called once per frame
    void Update()
    {
        float mouse_X = Input.GetAxis("Mouse X");
        mx += mouse_X * rotspeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, mx, 0);
    }
}
