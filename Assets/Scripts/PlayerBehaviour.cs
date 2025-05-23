using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject grid;
    private List<List<Transform>> listaTiles = new List<List<Transform>>();

    private int gridSize;

    public int hp = 20;
    public int maxHp = 20;

    public bool isPlaying = false;

    Vector3 SetZ(Vector3 vec, float z)
    {
        vec.z = z;
        return vec;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        }
    }

    // Update is called once per frame
    void Update()
    {
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
        }
    }
}
