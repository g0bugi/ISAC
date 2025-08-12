using UnityEngine;

public class AudioSubManage : MonoBehaviour
{
    public GameObject circlePrefab;
    int Count = 0;

    float timer = 0f;
    // Update is called once per frame
    void Update()
    {
        if(timer > 50.5f && Count == 0)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-16f, 16f), Random.Range(-9f, 9f), 0f);
            GameObject colorball = Instantiate(circlePrefab, spawnPos, Quaternion.identity);
            colorball.transform.localScale *= 5f;
            Count++;
        }
        if(timer > 52f && Count == 1)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-16f, 16f), Random.Range(-9f, 9f), 0f);
            GameObject colorball = Instantiate(circlePrefab, spawnPos, Quaternion.identity);
            colorball.transform.localScale *= 10f;
            Count++;
        }
        timer += Time.deltaTime;
    }
}
