using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Curtain : MonoBehaviour, IInteractiable
{
    public bool IsOutSpreaded = false;
    bool IsWorking = false;
    public void Action()
    {
        if(!IsWorking)
            StartCoroutine(Swap());
    }

    IEnumerator Swap()
    {
        IsWorking = true;

        Vector3 scale = transform.localScale;
        Vector3 position = transform.position;
        Vector3 startPos = transform.position;


        float activetime = 1f;
        float time = 0f;
        if (IsOutSpreaded) {
            while (activetime > time)
            {
                float t = time / activetime;

                scale.x = Mathf.Lerp(4, 2, t);
                transform.localScale = scale;

                // 기준점을 기준으로 이동
                transform.position = new Vector3(Mathf.Lerp(startPos.x, startPos.x + 1, t), startPos.y, startPos.z);

                time += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (activetime > time)
            {
                float t = time / activetime;

                scale.x = Mathf.Lerp(2, 4, t);
                transform.localScale = scale;

                transform.position = new Vector3(Mathf.Lerp(startPos.x, startPos.x - 1, t), startPos.y, startPos.z);

                time += Time.deltaTime;
                yield return null;
            }

        }
        IsOutSpreaded = !IsOutSpreaded;

        IsWorking = false;
        yield break;
    }
}
