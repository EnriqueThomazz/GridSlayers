using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class LoadCards : MonoBehaviour
{

    private List<List<string>> cartas = new List<List<string>>();

    public GameObject cardTemplate;

    public GameObject trash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Ler as cartas do CSV
        string filePath = Path.Combine(Application.streamingAssetsPath, "cards.csv");

        string[] cards = File.ReadAllLines(filePath);

        foreach (string card in cards)
        {
            string[] c = card.Split(",");

            if (c[0] != "Nome")
            {
                for (int i = 0; i < int.Parse(c[3].Trim()); i++)
                {
                    List<string> crd = new List<string>();
                    crd.Add(c[0]);
                    crd.Add(c[1]);
                    crd.Add(c[2]);
                    cartas.Add(crd);
                }
            }
        }

        // Instanciar as cartas
        for (int i=0; i < cartas.Count; i++)
        {
            Instantiate(cardTemplate, new Vector3(-5.0f + i * 2.0f, -5.0f, -1), Quaternion.identity, transform);
            transform.GetChild(i).gameObject.GetComponent<CardBehaviour>().cardContents = cartas[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i=0; i<transform.childCount; i++)
        {
            // Organizando as cartas pra ficar mais facil de debugar
            transform.GetChild(i).transform.position = new Vector3(-5.0f + i * 2.0f, -5.0f, -1);
        }
        
    }

    public void drawCard(Transform caller)
    {
        if (transform.childCount == 0) // Se ficou sem cartas no deck
        {
            trash.GetComponent<TrashManager>().refilDeck(transform); // Pede pro trash reembaralhar as cartas
        }

        int cardToDraw = Random.Range(0, transform.childCount); // Escolhe uma carta aleatória

        Transform card = transform.GetChild(cardToDraw);

        card.transform.SetParent(caller); // Muda o parentesco dela
    }
}
