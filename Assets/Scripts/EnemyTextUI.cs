using TMPro;
using UnityEngine;

public class EnemyTextUII : MonoBehaviour
{
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI dmgText;

    public GameObject enemy;

    public GameObject alive;
    public GameObject dead;

    public Sprite weapon_cc;
    public Sprite weapon_la;
    public GameObject weaponImg;

    void Start()
    {
        
    }

    void Update()
    {
        if (enemy != null) // Se o inimigo esta vivo
        {
            alive.SetActive(true);
            dead.SetActive(false);

            lifeText.text = enemy.transform.GetComponent<EnemyBehaviour>().hp.ToString();
            dmgText.text = enemy.transform.GetComponent<EnemyBehaviour>().dmg.ToString();

            if (enemy.transform.GetComponent<EnemyBehaviour>().atkType == "cc")
            {
                weaponImg.transform.GetComponent<SpriteRenderer>().sprite = weapon_cc;
            }
            else
            {
                weaponImg.transform.GetComponent<SpriteRenderer>().sprite = weapon_la;
            }

        }
        else // Se o inimigo esta morto
        {
            alive.SetActive(false);
            dead.SetActive(true);
        }

    }
}
