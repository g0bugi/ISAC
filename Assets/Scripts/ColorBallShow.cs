using JetBrains.Annotations;
using UnityEngine;

public class ColorBallShow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public GameObject ball_factory;
    float timer = 0.0f;
    // Update is called once per frame
    void Spawn()
    {
        Vector3 Randompos = new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), 0);
        GameObject ball = Instantiate(ball_factory, Randompos, Quaternion.identity);
    }
    void Update() 
    {

        timer += Time.deltaTime;
        if(timer > 3)
        {
            Spawn();
            timer = 0.0f;
        }
    }

    
}
