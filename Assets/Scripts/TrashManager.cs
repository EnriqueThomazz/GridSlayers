using UnityEngine;

public class TrashManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i=0; i<transform.childCount; i++)
        {
            // Organizando os filhos pra poder ver melhor na hr de debugar
            transform.GetChild(i).transform.position = new Vector3(-5.0f + i * 2.0f, 10.0f, -1);
        }        
    }

    public void refilDeck(Transform caller)
    {
        // Quando o deck ficar sem filhos ele chama esse método.
        // Envia todos os filhos de trash para o deck de forma aleatória
        Debug.Log("Reembaralhando cartas...");

        for (int i=0; i<transform.childCount; i++)
        {
            int card_index = Random.Range(0, transform.childCount-i); // Escolhe uma carta qqr

            transform.GetChild(card_index).SetParent(caller); // Envia a carta escolhida pro Deck
        }

    }
}
