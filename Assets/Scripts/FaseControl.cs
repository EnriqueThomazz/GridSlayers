using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FaseControl : MonoBehaviour
{
    public GameObject player;
    public GameObject grid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Colocando player no centro do grid
        grid.transform.GetChild(grid.transform.childCount / 2).gameObject.GetComponent<TileProperties>().setTop(player);

        // Spawnar inimigos (!!!)
    }

    // Update is called once per frame
    void Update()
    {
        // Comprar N cartas, onde N é igual a (5 - qtdCartasNaMao)

        // Definir isPlaying do player como true

        

        // Iterar pela lista de inimigos, definindo isPlaying deles como true
        // Como fazer para esperar o turno do coleguinha (???)

        // Se não tem mais inimigos, spawna um boss        
    }
}
