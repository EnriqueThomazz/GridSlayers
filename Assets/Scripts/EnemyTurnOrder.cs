using TMPro;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class EnemyTurnOrder : MonoBehaviour
{
    public GameObject enemyStats;
    public GameObject enemySpawner;

    public GameObject enemyText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float yPadding = -1.2f;
        int index = 0;
        // Iterar pela lista de inimigos (filhos de EnemySpawner)
        // e mostrar uma imagem dele, seu hp, seu dano e seu tipo de atk
        foreach (Transform enemy in enemySpawner.GetComponentInChildren<Transform>())
        {
            // Calculando a posição do enemyStats
            SpriteRenderer sr = GetComponent<SpriteRenderer>(); // Pra pegar a altura do retangulo
            Vector3 pos = new Vector3(transform.position.x - (sr.bounds.size.x / 2f) + 0.6f, transform.position.y + (sr.bounds.size.y / 2f) + yPadding * index - 0.7f, transform.position.z - 1);
            index++;

            // Instancia o enemyStats
            GameObject stats = Instantiate(enemyStats, pos, Quaternion.identity, transform);

            // Instancia o texto do inimigo
            GameObject text = Instantiate(enemyText, new Vector3(-11.5f, -1.7f - 1.2f * index, -2), Quaternion.identity, transform);
            text.transform.localScale = new Vector3(0.3f, 0.3f, 1f);

            // Passando o inimigo pro elemento visual
            text.transform.GetComponent<EnemyTextUII>().enemy = enemy.gameObject;


            // Carrega a imagem certa
            string profilePhoto = enemy.GetComponent<EnemyBehaviour>().spriteName + "_pf";

            // Coloca a imagem no SpriteRenderer de stats
            Sprite sprite = Resources.Load<Sprite>("Images/Enemies/profiles/" + profilePhoto);

            stats.GetComponent<SpriteRenderer>().sprite = sprite;

            stats.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
