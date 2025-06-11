using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit;
using UnityEngine;

public class FaseControl : MonoBehaviour
{
    public GameObject player;
    public GameObject grid;

    private Transform enemySpawner = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemySpawner = transform.Find("EnemySpawner");

        // Colocando player no centro do grid
        grid.transform.GetChild(grid.transform.childCount / 2).gameObject.GetComponent<TileProperties>().setTop(player);
        player.GetComponent<PlayerBehaviour>().gridPosition = grid.transform.childCount / 2;

        // Player é o primeiro a jogar
        player.transform.GetComponent<PlayerBehaviour>().isPlaying = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Comprar N cartas, onde N é igual a (5 - qtdCartasNaMao)

        // Definir isPlaying do player como true

        // Atualizando os turnos

        // Se é o turno do player
        if (player.transform.GetComponent<PlayerBehaviour>().isPlaying)
        {
            // Se o player disse que acabou seu turno
            if (player.transform.GetComponent<PlayerBehaviour>().turnEnded)
            {
                // Reseta os atributos de turno
                player.transform.GetComponent<PlayerBehaviour>().isPlaying = false;
                player.transform.GetComponent<PlayerBehaviour>().isPlaying = false;

                // Passa o turno pro primeiro inimigo
                if (enemySpawner != null)
                {
                    enemySpawner.GetChild(0).GetComponent<EnemyBehaviour>().isPlaying = true;
                }
            }

        }
        else if (enemySpawner != null) // Se não é o turno do player, é de algum inimigo
        {
                // Itera por todos os inimigos
                for (int i = 0; i < enemySpawner.childCount; i++)
                {
                    Transform enemy = enemySpawner.GetChild(i);

                    // Se achou um inimigo que está jogando
                    if (enemy.GetComponent<EnemyBehaviour>().isPlaying)
                    {
                        // Se aquele inimigo disse que terminou seu turno
                        if (enemy.GetComponent<EnemyBehaviour>().turnEnded)
                        {
                            // Reseta os atributos de turno do inimigo atual
                            enemy.GetComponent<EnemyBehaviour>().isPlaying = false;
                            enemy.GetComponent<EnemyBehaviour>().turnEnded = false;

                            // Então passa o turno pro próximo inimigo
                            if (i != enemySpawner.childCount - 1)
                            {
                                Transform next_enemy = enemySpawner.GetChild(i+1);
                                next_enemy.GetComponent<EnemyBehaviour>().isPlaying = true;

                            }
                            else // Significa que era o ultimo inimigo da lista
                            {
                                // Então é a vez do player
                                player.transform.GetComponent<PlayerBehaviour>().mana = 5;
                                player.transform.GetComponent<PlayerBehaviour>().isPlaying = true;
                                player.transform.GetComponent<PlayerBehaviour>().turnEnded = false;
                            }
                        }

                        break;
                    }
                }
            
        }




        

        // Iterar pela lista de inimigos, definindo isPlaying deles como true
        // Como fazer para esperar o turno do coleguinha (???)

        // Se não tem mais inimigos, spawna um boss        
    }
}
