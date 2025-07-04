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
        
    }

    public void generateTurnsUI()
    {
        // Limpa tudo que tem antes de continuar
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            if (child.tag != "undeletable") // Evita deletar o background
            {
                Destroy(child.gameObject);
            }
        }

        float yPadding = -1.2f;
        int index = 0;
        // Iterar pela lista de inimigos (filhos de EnemySpawner)
        // e mostrar uma imagem dele, seu hp, seu dano e seu tipo de atk
        foreach (Transform enemy in enemySpawner.GetComponentInChildren<Transform>())
        {
            // Calculando a posição do enemyStats
            SpriteRenderer sr = transform.GetChild(0).GetComponent<SpriteRenderer>(); // Pra pegar a altura do retangulo
            Vector3 pos = new Vector3(transform.position.x - (sr.bounds.size.x / 2f) + 0.8f, transform.position.y + (sr.bounds.size.y / 2f) + yPadding * index - 0.7f, transform.position.z - 1);
            index++;

            // Instancia o enemyStats
            GameObject stats = Instantiate(enemyStats, pos, Quaternion.identity, transform);

            // Instancia o texto do inimigo
            GameObject text = Instantiate(enemyText, pos + new Vector3(-3.4f, -3.6f, 0f), Quaternion.identity, transform);
            //text.transform.localScale = new Vector3(0.3f, 0.3f, 1f);

            // Passando o inimigo pro elemento visual
            text.transform.GetComponent<EnemyTextUII>().enemy = enemy.gameObject;


            // Carrega a imagem certa
            string profilePhoto = enemy.GetComponent<EnemyBehaviour>().spriteName + "_pf";

            // Coloca a imagem no SpriteRenderer de stats
            Sprite sprite = Resources.Load<Sprite>("Images/Enemies/profiles/" + profilePhoto);

            stats.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
