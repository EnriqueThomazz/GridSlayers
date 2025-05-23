using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CardBehaviour : MonoBehaviour
{
    public List<string> cardContents;

    private TextMeshProUGUI costText;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI effectText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (cardContents != null)
        {
            // Escrever o custo, nome e efeito da carta
            costText = transform.Find("Canvas/CostText").GetComponent<TextMeshProUGUI>();
            nameText = transform.Find("Canvas/NameText").GetComponent<TextMeshProUGUI>();
            effectText = transform.Find("Canvas/EffectText").GetComponent<TextMeshProUGUI>();

            costText.text = cardContents[2];
            nameText.text = cardContents[0];
            effectText.text = cardContents[1];
        }
    }
}
