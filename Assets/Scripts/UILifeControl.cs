using TMPro;
using UnityEngine;

public class UILifeControl : MonoBehaviour
{
    public GameObject player;

    public TextMeshProUGUI hpText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = player.GetComponent<PlayerBehaviour>().hp.ToString() + '/' + player.GetComponent<PlayerBehaviour>().maxHp.ToString();
    }
}
