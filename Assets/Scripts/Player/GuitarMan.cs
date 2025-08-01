using System.Runtime.CompilerServices;
using UnityEngine;

public class GuitarMan : MonoBehaviour
{
    public GameObject chair;
    public GameObject guitarman;
    public DialogManager manager;
    private bool IsSitting = false;
    public Playermove playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(IsSitting)
        {
            if(Input.GetKeyDown(KeyCode.Space))
                {
                manager.Action(guitarman);
            }
        }
    }
    void Sit()
    {
        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        transform.position = chair.transform.position;

        if (cc != null) cc.enabled = true;
        IsSitting = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == guitarman)
        {
            Debug.Log("Trigger: guitarman과 충돌함");
            playerMovement = GetComponent<Playermove>();
            playerMovement.SetCanMove(false);
            Sit();

            

            manager.Action(guitarman);
        }
    }

}
