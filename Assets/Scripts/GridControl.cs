using JetBrains.Annotations;
using UnityEngine;

public class GridControl : MonoBehaviour
{
    public GameObject tile;
    public float spacing = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float startX = Screen.width * 1.0f;
        float startY = Screen.height * 0.9f;

        Vector2 startPos = Camera.main.ScreenToWorldPoint(new Vector2(startX, startY));
        startPos.x -= 5 * spacing;

        float tileWidth = tile.GetComponent<SpriteRenderer>().bounds.size.x;

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Vector2 spawnPos = new Vector2(startPos.x + (j * spacing), startPos.y - (i * spacing));

                Instantiate(tile, spawnPos, Quaternion.identity, transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
