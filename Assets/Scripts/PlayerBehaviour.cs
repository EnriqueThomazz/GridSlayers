using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject grid;
    private List<List<Transform>> listaTiles = new List<List<Transform>>();
    public int gridPosition;

    private int gridSize;

    public int hp = 20;
    public int maxHp = 20;
    public int buffDmg = 0; // Pra cartas que buffam o dano, reseta qdo termina o turno
    public bool copyActive = false; // Pra carta de cópia

    public int mana = 2;

    public bool isPlaying = false; // Indica se é o turno do player (Setado por FaseControl)
    public bool turnEnded = false; // Indica se o turno do player acabou

    AudioManager audioManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        gridSize = (int) Mathf.Sqrt(grid.transform.childCount);

        int c = 0;
        while (c != grid.transform.childCount)
        {
            List<Transform> row = new List<Transform>();
            for (int i=0; i < gridSize; i++)
            {
                row.Add(grid.transform.GetChild(c+i).transform);
            }
            listaTiles.Add(row);
            c += 5;
        }
    }

    // Mudar a lógica de movimentação (!!!)
    void move(int x_dir, int y_dir)
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (listaTiles[i][j].gameObject.GetComponent<Collider2D>().OverlapPoint(this.transform.position))
                {
                    listaTiles[i][j].gameObject.GetComponent<TileProperties>().setTop(null);
                    listaTiles[i + y_dir][j + x_dir].gameObject.GetComponent<TileProperties>().setTop(this.gameObject);
                    gridPosition = (i + y_dir) * 5 + j + x_dir;
                    Debug.Log("Posição do player: " + gridPosition.ToString());
                }
            }
        }
    }

    public void takeDmg(int dmg)
    {
        hp = hp - dmg;

        if (hp <= 0)
        {
            hp = 0;
            Debug.Log("Player morto!");
            audioManager.playSFX(audioManager.gameOver);
            StartCoroutine(gameOver());
        }
    }

    System.Collections.IEnumerator gameOver()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("GameOver");
    }

    // Update is called once per frame
    void Update()
    {
        // Eventos para debug apenas...
        if (Input.GetKeyDown(KeyCode.A))
        {
            move(-1, 0);
        }else if (Input.GetKeyDown(KeyCode.D))
        {
            move(1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            move(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            move(0, 1);
        } else if (Input.GetKeyDown(KeyCode.Space))
        {
            buffDmg = 0;
            copyActive = false;
            turnEnded = true; // Finaliza o turno
        } else if (Input.GetKeyDown(KeyCode.K)) // Mata todos os inimigos na tela
        {
            for (int i=0; i<grid.transform.childCount; i++)
            {
                Transform tile = grid.transform.GetChild(i);
                if (tile.GetComponent<TileProperties>().onTop)
                {
                    if (tile.GetComponent<TileProperties>().onTop != transform.gameObject)
                    {
                        tile.GetComponent<TileProperties>().onTop.transform.GetComponent<EnemyBehaviour>().hp = 0;
                    }
                }
            }
        }
    }

    public void endTurn()
    {
        // Reseta variáveis de cartas
        buffDmg = 0;
        copyActive = false;

        // Finaliza o turno
        turnEnded = true;
    }
}
