using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Enable);
    }
    public void Enable()
    {
        Debug.Log("버튼 동작함");
    }
}
