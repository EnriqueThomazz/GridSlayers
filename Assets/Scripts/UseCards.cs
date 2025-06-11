using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor.Analytics;
using UnityEngine;

public class UseCards : MonoBehaviour
{
    public GameObject deck; // deck de cartas
    public GameObject trash; // cartas usadas v�o pro lixo

    public GameObject player; // pra manipular os atributos

    public GameObject grid;

    public GameObject enemies;

    public int tileClicked = -1;
    public Sprite tileClickedSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tileClicked = -1;        
    }

    // Update is called once per frame
    void Update()
    {
        // Player s� pode ter at� 4 cartas na m�o

        // Atualizar os transform das cartas na m�o dinamicamente
        // de forma que as cartas sempre fiquem juntas

        // Player pode escolher usar cartas e fazer as diversas a��es (muitos if/else)


        // Comprando cartas ====================================
        if (player.GetComponent<PlayerBehaviour>().isPlaying && !player.GetComponent<PlayerBehaviour>().turnEnded) // Indica que � o come�o do turno do player
        {
            for (int i=0; i<4-transform.childCount; i++) // V� quantas cartas tem que comprar
            {
                deck.GetComponent<LoadCards>().drawCard(transform); // Compra uma carta
            }
        }
        // ====================================================


        // Ajustando a posi��o das cartas na m�o
        for (int i=0; i<transform.childCount; i++)
        {
            Transform card = transform.GetChild(i);

            Vector3 basePosition = new Vector3(1.25f + i * 0.2f, 0.4f, 0f); // Posi��o base na m�o

            if (verifyMouseHover(card)) // Se o mouse passou por cima da carta
            {
                card.localPosition = basePosition + new Vector3(0f, 0.05f, 0f); // sobe um pouco o Y

                // Usando cartas %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                if (Input.GetMouseButtonDown(0))
                {
                    // Tenta usar a carta, se usou manda pro trash
                    if (cardEffect(card.GetComponent<CardBehaviour>().nameText.text))
                    {
                        card.transform.SetParent(trash.transform);
                    }
                }
                // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
            }
            else
            {
                card.localPosition = basePosition;
            }
        }        
    }

    private bool verifyMouseHover(Transform card)
    {
        Collider2D col = card.GetComponentInChildren<Collider2D>(); // Pegando o collider do sprite dentro do objeto card

        if (col == null) return false;

        // Converte a posi��o do mouse para o mundo
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; // tava bugando, tive que fzr isso pra parar

        //.DrawRay(mouseWorldPos, Vector3.forward * 5, Color.red, 1f);
        //Debug.DrawLine(mouseWorldPos - Vector3.up * 0.1f, mouseWorldPos + Vector3.up * 0.1f, Color.red, 1f);


        // Se o mouse ta sobre a carta
        if (col.OverlapPoint(mouseWorldPos))
        {
            return true;
        }

        // Sen�o
        return false;
    }

    private bool cardEffect(string cardName)
    {
        int playerPos = player.GetComponent<PlayerBehaviour>().gridPosition; // Posi��o do player no grid

        Debug.Log("Carta usada " + cardName);

        switch (cardName)
        {
            // ok
            case "Passos R�pidos":
                // Mova 1 casa em qualquer dire��o

                // Verifica se o tile selecionado � adjacente ao tile do player
                // e se n�o tem nada onTop. Se passar por tudo isso, move o player pra l�

                // Custa 0

                if (tileClicked == -1)
                {
                    Debug.Log("Nenhum tile selecionado!");
                    return false;
                }

                // Verificando se o tile selecionado � uma posi��o v�lida
                if (tileClicked % 5 == playerPos % 5 || tileClicked / 5 == playerPos / 5) // Verifica se est� na mesma linha/coluna
                {
                    // Verificando adjac�ncia
                    if (tileClicked == playerPos - 1 || tileClicked == playerPos + 1 || tileClicked == playerPos + 5 || tileClicked == playerPos - 5)
                    {
                        if (grid.transform.GetChild(tileClicked).GetComponent<TileProperties>().onTop == null) // Se n�o tiver nada em cima do tile
                        {
                            player.GetComponent<PlayerBehaviour>().gridPosition = tileClicked;
                            grid.transform.GetChild(playerPos).GetComponent<TileProperties>().onTop = null; // Tira o player de onde ele t�
                            grid.transform.GetChild(tileClicked).GetComponent<TileProperties>().onTop = player.gameObject; // Move o player pro novo tile
                            tileClicked = -1; // Reseta tileClicked
                        }
                        else
                        {
                            Debug.Log("Existe algo em cima desse tile!");
                            return false;
                        }
                    }
                }
                else
                {
                    Debug.Log("N�o foi selecionado um tile v�lido!");
                    return false;
                }

                    break;

            // ok
            case "Soco":
                // Cause 5 de dano a um inimigo adjacente

                if (tileClicked == -1)
                {
                    Debug.Log("Nenhum tile selecionado!");
                    return false;
                }

                // Verificando se o tile selecionado � uma posi��o v�lida
                if (tileClicked % 5 == playerPos % 5 || tileClicked / 5 == playerPos / 5) // Verifica se est� na mesma linha/coluna
                {
                    // Verificando adjac�ncia
                    if (tileClicked == playerPos - 1 || tileClicked == playerPos + 1 || tileClicked == playerPos + 5 || tileClicked == playerPos - 5)
                    {
                        if (grid.transform.GetChild(tileClicked).GetComponent<TileProperties>().onTop != null) // Se tem algo em cima do tile
                        {
                            // Verificando a mana
                            if (!(player.GetComponent<PlayerBehaviour>().mana >= 1)) return false; // Se o player n tem mana o suficiente retorna 
                            player.GetComponent<PlayerBehaviour>().mana -= 1; // Gasta a mana

                            // Pega o inimigo que est� em cima, deveria verificar se � um inimigo (!!!)
                            EnemyBehaviour enemyB = grid.transform.GetChild(tileClicked).GetComponent<TileProperties>().onTop.GetComponent<EnemyBehaviour>();

                            enemyB.hp -= 5 + player.GetComponent<PlayerBehaviour>().buffDmg; // Causa dano no inimigo

                            tileClicked = -1; // Reseta tileClicked
                        }
                        else
                        {
                            Debug.Log("N�o h� um inimigo nesse tile!");
                            return false;
                        }
                    }
                }
                else
                {
                    Debug.Log("N�o foi selecionado um tile v�lido!");
                    return false;
                }

                break;

            // ok
            case "Chute Duplo":
                // Tenta causar dano nos inimigos adjacentes na mesma linha
                // Mesma linha = mesma divis�o inteira por 5

                if (!(player.GetComponent<PlayerBehaviour>().mana >= 2)) return false; // Se o player n tem mana o suficiente retorna false

                player.GetComponent<PlayerBehaviour>().mana -= 2; // Gasta a mana

                int before = ((playerPos - 1) / 5 == playerPos / 5) ? playerPos - 1 : -1;
                int after = ((playerPos + 1) / 5 == playerPos / 5) ? playerPos + 1 : -1;

                if (before != -1)
                {
                    Transform tile = grid.transform.GetChild(before);
                    if (tile.GetComponent<TileProperties>().onTop) // Se tem algo no tile
                    {
                        tile.GetComponent<TileProperties>().onTop.GetComponent<EnemyBehaviour>().hp -= 5 + player.GetComponent<PlayerBehaviour>().buffDmg; // Causa dano no inimigo (precisaria verificar se � de fato um inimigo !!!)
                    }
                }

                if (after != -1)
                {
                    Transform tile = grid.transform.GetChild(after);
                    if (tile.GetComponent<TileProperties>().onTop) // Se tem algo no tile
                    {
                        tile.GetComponent<TileProperties>().onTop.GetComponent<EnemyBehaviour>().hp -= 5 + player.GetComponent<PlayerBehaviour>().buffDmg; // Causa dano no inimigo (precisaria verificar se � de fato um inimigo !!!)
                    }
                }

                break;

            // ok
            case "Fortalecimento":
                if (!(player.GetComponent<PlayerBehaviour>().mana >= 1)) return false; // Se o player n tem mana o suficiente retorna false

                player.GetComponent<PlayerBehaviour>().mana -= 1; // Gasta a mana

                player.GetComponent<PlayerBehaviour>().buffDmg += 5;
                break;

            // ok
            case "Medita��o":
                player.GetComponent<PlayerBehaviour>().hp += 5;
                if (player.GetComponent<PlayerBehaviour>().hp > player.GetComponent<PlayerBehaviour>().maxHp) // Evitando curar mais do que o hp maximo
                {
                    player.GetComponent<PlayerBehaviour>().hp = player.GetComponent<PlayerBehaviour>().maxHp;
                }
                break;

            // ok
            case "Raiva":
                // Cause 10 de dano a todos os inimigos adjacentes. Perca 5 PV.


                // Mesma coluna = mesmo resto de divis�o por 5
                // Mesma linha = mesma divis�o inteira por 5

                if (!(player.GetComponent<PlayerBehaviour>().mana >= 2)) return false; // Se o player n tem mana o suficiente retorna false

                player.GetComponent<PlayerBehaviour>().mana -= 2; // Gasta a mana

                int l_before = ((playerPos - 1) / 5 == playerPos / 5) ? playerPos - 1 : -1; // Antes na mesma linha
                int l_after = ((playerPos + 1) / 5 == playerPos / 5) ? playerPos + 1 : -1; // Depois na mesma linha

                int c_before = ((playerPos - 5) % 5 == playerPos % 5) ? playerPos - 5 : -1; // Antes na mesma coluna
                int c_after = ((playerPos + 5) % 5 == playerPos % 5) ? playerPos + 5 : -1; // Depois na mesma coluna


                if (l_before != -1)
                {
                    Transform tile = grid.transform.GetChild(l_before);
                    if (tile.GetComponent<TileProperties>().onTop) // Se tem algo no tile
                    {
                        tile.GetComponent<TileProperties>().onTop.GetComponent<EnemyBehaviour>().hp -= 10 + player.GetComponent<PlayerBehaviour>().buffDmg;
                    }
                }

                if (l_after != -1)
                {
                    Transform tile = grid.transform.GetChild(l_after);
                    if (tile.GetComponent<TileProperties>().onTop) // Se tem algo no tile
                    {
                        tile.GetComponent<TileProperties>().onTop.GetComponent<EnemyBehaviour>().hp -= 10 + player.GetComponent<PlayerBehaviour>().buffDmg;
                    }
                }

                if (c_before > -1) // Pode ser que tenha olhado em um valor negativo
                {
                    Transform tile = grid.transform.GetChild(c_before);
                    if (tile.GetComponent<TileProperties>().onTop) // Se tem algo no tile
                    {
                        tile.GetComponent<TileProperties>().onTop.GetComponent<EnemyBehaviour>().hp -= 10 + player.GetComponent<PlayerBehaviour>().buffDmg;
                    }
                }

                if (c_after > -1) // Pode ser que tenha olhado em um valor negativo
                {
                    Transform tile = grid.transform.GetChild(c_after);
                    if (tile.GetComponent<TileProperties>().onTop) // Se tem algo no tile
                    {
                        tile.GetComponent<TileProperties>().onTop.GetComponent<EnemyBehaviour>().hp -= 10 + player.GetComponent<PlayerBehaviour>().buffDmg;
                    }
                }

                // Por fim, o dano no player
                player.GetComponent<PlayerBehaviour>().hp -= 5;

                break;

            // ok
            case "Atrair":
                // Mova todos os inimigos 1 casa na sua dire��o

                // Basta chamar o moveTowardsPlayer do inimigo

                if (!(player.GetComponent<PlayerBehaviour>().mana >= 1)) return false; // Se o player n tem mana o suficiente retorna false

                player.GetComponent<PlayerBehaviour>().mana -= 1; // Gasta a mana

                for (int i=0; i<enemies.transform.childCount; i++) // Passa por todos os inimigos
                {
                    Transform enemy = enemies.transform.GetChild(i); // Pega o inimigo

                    for (int j=0; j<grid.transform.childCount; j++) // Iterando pelos tiles pq precisa da pos do inimigo
                    {
                        if (grid.transform.GetChild(j).GetComponent<TileProperties>().onTop == enemy.gameObject)
                        {
                            enemy.GetComponent<EnemyBehaviour>().moveTowardsPlayer(j, playerPos);
                            break;
                        }
                    }
                    
                }

                break;

            // ok
            case "Repelir":
                // Mova todos os inimigos 1 casa para longe de voc�

                // Basta chamar o moveAwayPlayer do inimigo

                if (!(player.GetComponent<PlayerBehaviour>().mana >= 2)) return false; // Se o player n tem mana o suficiente retorna false
                player.GetComponent<PlayerBehaviour>().mana -= 2; // Gasta a mana

                for (int i = 0; i < enemies.transform.childCount; i++) // Passa por todos os inimigos
                {
                    Transform enemy = enemies.transform.GetChild(i); // Pega o inimigo

                    for (int j = 0; j < grid.transform.childCount; j++) // Iterando pelos tiles pq precisa da pos do inimigo
                    {
                        if (grid.transform.GetChild(j).GetComponent<TileProperties>().onTop == enemy.gameObject)
                        {
                            enemy.GetComponent<EnemyBehaviour>().moveAwayPlayer(j, playerPos);

                            break;
                        }
                    }

                }

                break;

            // ok
            case "C�pia":
                // A pr�xima carta ser� utilizada duas vezes

                if (!(player.GetComponent<PlayerBehaviour>().mana >= 2)) return false; // Se o player n tem mana o suficiente retorna false

                player.GetComponent<PlayerBehaviour>().mana -= 2; // Gasta a mana

                player.GetComponent<PlayerBehaviour>().copyActive = true; // Define que c�pia est� ativa
                break;

            // ok
            case "Golpe Fatal":
                // Cause 10 de dano a um inimigo adjacente

                if (tileClicked == -1)
                {
                    Debug.Log("Nenhum tile selecionado!");
                    return false;
                }

                // Verificando se o tile selecionado � uma posi��o v�lida
                if (tileClicked % 5 == playerPos % 5 || tileClicked / 5 == playerPos / 5) // Verifica se est� na mesma linha/coluna
                {
                    // Verificando adjac�ncia
                    if (tileClicked == playerPos - 1 || tileClicked == playerPos + 1 || tileClicked == playerPos + 5 || tileClicked == playerPos - 5)
                    {
                        if (grid.transform.GetChild(tileClicked).GetComponent<TileProperties>().onTop != null) // Se tem algo em cima do tile
                        {
                            // Verificando a mana
                            if (!(player.GetComponent<PlayerBehaviour>().mana >= 3)) return false; // Se o player n tem mana o suficiente retorna 
                            player.GetComponent<PlayerBehaviour>().mana -= 3; // Gasta a mana

                            // Pega o inimigo que est� em cima, deveria verificar se � um inimigo (!!!)
                            EnemyBehaviour enemyB = grid.transform.GetChild(tileClicked).GetComponent<TileProperties>().onTop.GetComponent<EnemyBehaviour>();

                            enemyB.hp -= 10 + player.GetComponent<PlayerBehaviour>().buffDmg; // Causa dano no inimigo

                            tileClicked = -1; // Reseta tileClicked
                        }
                        else
                        {
                            Debug.Log("N�o h� um inimigo nesse tile!");
                            return false;
                        }
                    }
                }
                else
                {
                    Debug.Log("N�o foi selecionado um tile v�lido!");
                    return false;
                }

                break;

            // ok
            case "Soco Abrangente":
                // Cause 3 de dano a todos os inimigos na mesma coluna que voc�

                // Mesma coluna = mesmo resto de divis�o por 5
                // Passar por todos os tiles e ver se est�o na mesma coluna do player

                if (!(player.GetComponent<PlayerBehaviour>().mana >= 2)) return false; // Se o player n tem mana o suficiente retorna false

                player.GetComponent<PlayerBehaviour>().mana -= 2; // Gasta a mana

                for (int i=0; i<grid.transform.childCount; i++)
                {
                    if (i != playerPos && i % 5 == playerPos % 5) // Se n�o � o player e est� na mesma coluna
                    {
                        Transform tile = grid.transform.GetChild(i);
                        if (tile.GetComponent<TileProperties>().onTop) // Se tem algo no tile
                        {
                            // Causa dano
                            tile.GetComponent<TileProperties>().onTop.GetComponent<EnemyBehaviour>().hp -= 3 + player.GetComponent<PlayerBehaviour>().buffDmg;
                        }
                    }
                }

                break;

            // ok
            case "Chute Abrangente":
                //Cause 3 de dano a todos os inimigos na mesma linha que voc�

                // Mesma linha = mesma divis�o inteira por 5
                // Calcular (player - 4), (player + 4)

                if (!(player.GetComponent<PlayerBehaviour>().mana >= 2)) return false; // Se o player n tem mana o suficiente retorna false

                player.GetComponent<PlayerBehaviour>().mana -= 2; // Gasta a mana

                for (int i = ((playerPos - 4 >= 0) ? playerPos - 4 : 0); i < ((playerPos + 4 <= 24) ? playerPos + 4 : 24); i++) // Passa por todas as posi��es
                {
                    if (i / 5 == playerPos / 5 && i != playerPos) // Se � uma posi��o na mesma linha
                    {
                        Transform tile = grid.transform.GetChild(i);
                        if (tile.GetComponent<TileProperties>().onTop) // Se tem algo no tile
                        {
                            Debug.Log("Casuando dano...");
                            // Causa dano
                            tile.GetComponent<TileProperties>().onTop.GetComponent<EnemyBehaviour>().hp -= 3 + player.GetComponent<PlayerBehaviour>().buffDmg;
                        }
                    }
                }

                break;

            default:
                Debug.Log("Efeito de carta n�o encontrado!");
                break;
        }

        if (player.GetComponent<PlayerBehaviour>().copyActive && cardName != "C�pia") // Se o player estava com c�pia ativa
        {
            player.GetComponent<PlayerBehaviour>().copyActive = false; // Desativa a c�pia
            cardEffect(cardName); // Chama recursivamente o efeito da carta
        }

        return true;
    }

}
