using TMPro;
using UnityEngine;

public class UIManaControl : MonoBehaviour
{
    public GameObject player;

    public TextMeshProUGUI manaText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        manaText.text = player.GetComponent<PlayerBehaviour>().mana.ToString() + "/5";
    }
}
