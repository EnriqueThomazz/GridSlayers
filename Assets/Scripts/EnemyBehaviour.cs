using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour
{
    public bool isPlaying = false; // Indica se é o turno do inimigo (setado por FaseControl)
    public bool turnEnded = false; // Indica se o turno acabou

    bool turnInProgress = false; // Define se o inimigo está "jogando" o turno (util na corrotina)

    private GameObject grid;
    private GameObject player;
    public int hp;
    public int dmg;
    private string atkType;
    public string spriteName;

    public SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying && !turnInProgress && !turnEnded)
        {
            StartCoroutine(ExecutarTurno());
        }
    }

    IEnumerator ExecutarTurno()
    {
        turnInProgress = true;

        int playerPos = 0;
        int myPos = 0;

        // Calcula a minha posicao e a posicao do player
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            Transform tile = grid.transform.GetChild(i);

            if (tile.GetComponent<TileProperties>().onTop == gameObject)
            {
                myPos = i;
            }

            if (tile.GetComponent<TileProperties>().onTop == player)
            {
                playerPos = i;
            }
        }

        // Verifica se está no alcance do player
        if (isPlayerReachable(myPos, playerPos))
        {
            player.GetComponent<PlayerBehaviour>().takeDmg(dmg);
        }
        else // Se não está, se move na direção do player
        {
            moveTowardsPlayer(myPos, playerPos);
        }

        // Espera 1 segundo antes de encerrar o turno
        yield return new WaitForSeconds(1f);

        turnEnded = true;
        turnInProgress = false;
    }

    private void moveTowardsPlayer(int myPos, int playerPos)
    {
        // Lógica: se estiver na mesma linha que o player, se move 1 tile na horizontal
        // Se estiver na mesma coluna, se move 1 tile na vertical
        int newPos = 0;
        if (myPos/5 == playerPos / 5) // Mesma linha
        {
            if (playerPos > myPos)
            {
                newPos = myPos + 1;
            }else if (playerPos < myPos)
            {
                newPos = myPos - 1;
            }

        } else if (myPos%5 == playerPos % 5) // Mesma coluna
        {
            if (playerPos > myPos)
            {
                newPos = myPos + 5;
            }
            else if (playerPos < myPos)
            {
                newPos = myPos - 5;
            }

        } else // Se não estiver nem na mesma coluna nem na mesma linha, move 1 tile aleatoriamente na vertical ou horizontal
        {
            string dir = UnityEngine.Random.Range(0, 2) == 0 ? "hor" : "ver";
            if (dir == "hor")
            {
                // Problema que poderia acontecer: player esta na posição seguinte, que é em outra linha
                // então me movimento para chegar mais perto -> vou para o mesmo tile que o player.
                // Logo, se eu verificar que vou para o mesmo tile que o player, em vez disso me movo na vertical.                
                if (playerPos%5 > myPos%5)
                {
                    if (playerPos != myPos + 1) newPos = myPos + 1;
                    else dir = "ver";
                }
                else if (playerPos%5 < myPos%5)
                {
                    if (playerPos != myPos - 1) newPos = myPos - 1;
                    else dir = "ver";
                }
            }
            if (dir == "ver")
            {
                if (playerPos > myPos)
                {
                    newPos = myPos + 5;
                }
                else if (playerPos < myPos)
                {
                    newPos = myPos - 5;
                }
            }
        }


        // Tendo o newPos calculado, basta alterar os atributos onTop do tile correto

        // Antes de mover, verifica se não esta tentando ir para um tile em que ja existe outro inimigo
        for (int i=0; i < transform.parent.childCount; i++) // Itera pelos irmãos
        {
            Transform filho = transform.parent.GetChild(i);

            if (filho != transform) // Verifica se não é ele mesmo
            {
                for (int j = 0; j < grid.transform.childCount; j++) // Itera pelos tiles pra pegar a posicao do irmao
                {
                    Transform tile = grid.transform.GetChild(j);

                    if (tile.GetComponent<TileProperties>().onTop == filho.gameObject) // Se o objeto onTop for o irmao
                    {
                        if (j == newPos) // Se a posicao do irmao for igual a que eu estou tentando ir
                        {
                            return; // Então retorna sem mover
                        }
                    }
                }
            }
        }
        // Se passou pelo for acima sem retornar, significa que o caminho está livre.
        // Então, move o inimigo
        grid.transform.GetChild(myPos).gameObject.GetComponent<TileProperties>().setTop(null);
        grid.transform.GetChild(newPos).gameObject.GetComponent<TileProperties>().setTop(gameObject);
    }

    private bool isPlayerReachable(int myPos, int playerPos)
    {
        // Verifica se estou no alcance de um ataque
        if (atkType == "cc") // Corpo a corpo
        {
            // Lógica: calcular a posição -1/+1 e executar a dvisão inteira delas por 5. Calcula também a divisão inteira de myPos por 5
            // Se forem iguais, eu estou no alcance corpo a corpo do player
            // Isso serve pra alcance horizontal
            int posMinus1 = playerPos - 1;
            int posPlus1 = playerPos + 1;

            // Lógica: calcular a posição -5/+5 e verificar se estão dentro do grid (< 0, > 25)
            // Isso serve para alcance vertical
            int posMinus5 = playerPos - 5;
            int posPlus5 = playerPos + 5;

            if ((myPos == posMinus1 & myPos / 5 == posMinus1 / 5) | (myPos == posPlus1 & myPos / 5 == posPlus1 / 5)) // Alcance horizontal
            {
                return true;
            }
            else if (myPos == posMinus5 | myPos == posPlus5) // Alcance vertical, não precisa verificar se esta no grid pois myPos sempre estará
            {
                return true;
            }

        }
        else if (atkType == "la") // Longo alcance
        {
            // Lógica: verificar se eu estou na mesma linha ou coluna do player
            // Mesma coluna: os restos da divisão por 5 têm de ser iguais
            // Mesma linha: a divisão inteira por 5 tem de ser igual
            if (myPos % 5 == playerPos % 5 | myPos / 5 == playerPos / 5)
            {
                return true;
            }

        }
        return false;
    }

    public void setAttr(int hp, int dmg, string atkType, string spriteName, GameObject grid, GameObject player)
    {
        this.hp = hp;
        this.dmg = dmg;
        this.atkType = atkType;
        this.spriteName = spriteName;
        this.grid = grid;
        this.player = player;

        Sprite sprite = Resources.Load<Sprite>("Images/Enemies/sprites/" + spriteName);

        spriteRenderer.sprite = sprite;

        transform.localScale = new Vector3(6f, 6f, 1f);
    }
}
