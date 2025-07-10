using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UIElements;
using UnityEngine.Rendering;
using System;
using UnityEngine.SceneManagement;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject grid;
    public GameObject enemyPrefab;
    public GameObject player;
    public GameObject enemyTurnOrder;
    public GameObject tile_deactivated;

    AudioManager audioManager;

    private List<List<string>> waves = new List<List<string>>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        TextAsset fase = Resources.Load<TextAsset>("Fases/fase_1");

        string[] enemies = fase.text.Split('\n', StringSplitOptions.RemoveEmptyEntries); // @ é o que define o intervalo entre waves

        List<string> wave = new List<string>();
        foreach (string enemy in enemies)
        {
            if (enemy.Trim() != "@")
            {
                wave.Add(enemy);
            }
            else
            {
                waves.Add(wave);
                wave = new List<string>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount <= 0) // Se não tem mais inimigos
        {
            if (spawnEnemies() == -1)
            {
                // Decreta o fim de jogo
                Debug.Log("Fase Concluida!");
                audioManager.playSFX(audioManager.faseComplete);
                StartCoroutine(fimDeFase());
            }
            else
            {
                // Atualiza o turno dos inimigos na UI
                enemyTurnOrder.transform.GetComponent<EnemyTurnOrder>().generateTurnsUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.K)) // Mata todos os inimigos na tela
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<EnemyBehaviour>().hp = 0;
            }
        }
    }
    public int spawnEnemies()
    {
        if (waves.Count <= 0) // N tem mais inimigos pra spawnar
        {
            return -1;
        }

        // Reseta a posição do player
        grid.transform.GetChild(player.GetComponent<PlayerBehaviour>().gridPosition).GetComponent<TileProperties>().onTop = null; // Tira o player de onde ele tá
        player.GetComponent<PlayerBehaviour>().gridPosition = 12;
        grid.transform.GetChild(12).GetComponent<TileProperties>().onTop = player.gameObject; // Move o player pro novo tile

        List<string> enemies = waves[0]; // Pega a prox wave

        // Instancia os inimigos
        foreach (string enemy in enemies)
        {
            // Pegando as propriedades
            string[] e = enemy.Split(",");
            
            if (e[0] != "tile")
            {
                // Instanciando um inimigo
                GameObject newEnemy = Instantiate(enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
                if (e[5].Trim() == "boss")
                {
                    newEnemy.transform.localScale = new Vector3(5f, 5f, 1f);
                    
                    // Instancia tiles desativados em cima dos tiles da lateral
                    for (int i=0; i<grid.transform.childCount; i++)
                    {
                        if (i % 5 == 0 || i % 5 == 4) // Primeira e ultima coluna
                        {
                            Transform tile = grid.transform.GetChild(i);

                            GameObject inactive_tile = Instantiate(tile_deactivated, new Vector3(0, 0, 0), Quaternion.identity, tile);

                            tile.GetComponent<TileProperties>().onTop = inactive_tile;
                        }
                    }
                }

                // Colando em cima do tile correto
                grid.transform.GetChild(int.Parse(e[0])).gameObject.GetComponent<TileProperties>().setTop(newEnemy);

                // Definindo os atributos do inimigo
                //newEnemy.GetComponent<EnemyBehaviour>().setAttr(int.Parse(e[2]), int.Parse(e[3]), e[4], e[1], grid, int.Parse(e[0]), player);
                newEnemy.GetComponent<EnemyBehaviour>().setAttr(int.Parse(e[2]), int.Parse(e[3]), e[4].Trim(), e[1], grid, player);
            }
        }

        waves.Remove(enemies); // Remove a wave da lista

        return 1;
    }

    System.Collections.IEnumerator fimDeFase()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("Buying");
    }
}
