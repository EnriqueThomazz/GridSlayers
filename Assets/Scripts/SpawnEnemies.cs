using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject grid;
    public GameObject enemyPrefab;
    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        spawnEnemies("./Assets/Fases/fase_1.txt");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void spawnEnemies(string filePath)
    {
        string[] enemies = File.ReadAllLines(filePath);

        foreach (string enemy in enemies)
        {
            // Pegando as propriedades
            string[] e = enemy.Split(",");
            
            if (e[0] != "tile")
            {
                // Instanciando um inimigo
                GameObject newEnemy = Instantiate(enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);

                // Colando em cima do tile correto
                grid.transform.GetChild(int.Parse(e[0])).gameObject.GetComponent<TileProperties>().setTop(newEnemy);

                // Definindo os atributos do inimigo
                //newEnemy.GetComponent<EnemyBehaviour>().setAttr(int.Parse(e[2]), int.Parse(e[3]), e[4], e[1], grid, int.Parse(e[0]), player);
                newEnemy.GetComponent<EnemyBehaviour>().setAttr(int.Parse(e[2]), int.Parse(e[3]), e[4], e[1], grid, player);
            }
        }
    }
}
