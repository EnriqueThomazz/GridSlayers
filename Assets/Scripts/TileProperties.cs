using UnityEngine;

public class TileProperties : MonoBehaviour
{
    public GameObject onTop = null;

    public Sprite normalSprite;
    public Sprite hoverSprite;
    public Sprite selectedSprite;

    public GameObject hand;

    Vector3 SetZ(Vector3 vec, float z)
    {
        vec.z = z;
        return vec;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Movendo quem esta no topo para ficar em cima do tile
        if (onTop != null)
        {
            onTop.transform.position = transform.position;
            onTop.transform.position = SetZ(onTop.transform.position, -1);
        }

        int clicked = hand.GetComponent<UseCards>().tileClicked;

        if (verifyMouseHover())// Verifica se o mouse do player esta em cima do tile
        {
            // Se não tem nenhum tile selecionad OU o tile selecionado não é esse, desenha o hoverSprite
            if (clicked == -1 || transform.parent.GetChild(hand.GetComponent<UseCards>().tileClicked) != transform) transform.GetComponent<SpriteRenderer>().sprite = hoverSprite;
            // Caso o Tile seja o que está selecionado, coloca o sprite de selecionado
            else if (clicked != -1) transform.GetComponent<SpriteRenderer>().sprite = selectedSprite;

            if (Input.GetMouseButtonDown(0)) // Se o player clicou
            {
                for (int i = 0; i < transform.parent.transform.childCount; i++) // Envia o index do tile pro UseCards
                {
                    if (transform.parent.transform.GetChild(i) == transform)
                    {
                        if (clicked == i) hand.GetComponent<UseCards>().tileClicked = -1; // Significa que ja tava selecionado, entao desseleciona
                        else
                        {
                            Debug.Log("Tile selecionado: " + i.ToString());
                            hand.GetComponent<UseCards>().tileClicked = i; // Caso contrário, seleciona
                        }
                        

                        break;
                    }
                }
            }
        }
        else
        {
            // Se não é o sprite que ta selecionad ou clicked é -1 (nenhum selecionado)
            if (clicked == -1 || transform.parent.GetChild(hand.GetComponent<UseCards>().tileClicked) != transform)
            {
                // Coloca o sprite padrão
                transform.GetComponent<SpriteRenderer>().sprite = normalSprite;
            }
        }
    }

    public void setTop(GameObject gameObj)
    {
        onTop = gameObj;
    }

    private bool verifyMouseHover()
    {
        Collider2D col = transform.GetComponent<Collider2D>(); // Pegando o collider do sprite dentro do objeto card

        if (col == null) return false;

        // Converte a posição do mouse para o mundo
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; // tava bugando, tive que fzr isso pra parar

        // Se o mouse ta sobre ao tile
        if (col.OverlapPoint(mouseWorldPos))
        {
            return true;
        }

        // Senão
        return false;
    }
}
