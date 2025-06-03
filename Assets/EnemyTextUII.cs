using TMPro;
using UnityEngine;

public class EnemyTextUII : MonoBehaviour
{
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI dmgText;

    public GameObject enemy;

    void Start()
    {
        
    }

    void Update()
    {
        if (enemy != null)
        {
            lifeText.text = enemy.transform.GetComponent<EnemyBehaviour>().hp.ToString();
            dmgText.text = enemy.transform.GetComponent<EnemyBehaviour>().dmg.ToString();
        }
        
    }
}
