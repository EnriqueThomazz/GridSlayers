using UnityEngine;
using UnityEngine.UI;

public class EndButtonControl : MonoBehaviour
{
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerBehaviour>().turnEnded)
        {
            transform.GetComponent<Button>().interactable = false;
        }
        else
        {
            transform.GetComponent<Button>().interactable = true;
        }     
    }
}
