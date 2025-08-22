using UnityEngine;

public class AudioSubManage : MonoBehaviour
{
    public Sprite specificRippleSprite;
    public SoundLight rippleManager;
    public GameObject circlePrefab;
    int Count = 0;

    float timer = 0f;
    // Update is called once per frame
    void Update()
    {
        if(timer > 50.5f && Count == 0)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-10f, 10f), Random.Range(-5f, 5f), 0f);
            
            rippleManager.PlayRippleEffect(spawnPos , 1000, specificRippleSprite);
           
            Count++;
        }
        if(timer > 52f && Count == 1)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-10f, 10f), Random.Range(-5f, 5f), 0f);

            rippleManager.PlayRippleEffect(spawnPos, 1000, specificRippleSprite);

            Count++;
        }
        timer += Time.deltaTime;
    }
}
