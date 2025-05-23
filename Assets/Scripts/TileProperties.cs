using UnityEngine;

public class TileProperties : MonoBehaviour
{
    public GameObject onTop = null;

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
        if (onTop != null)
        {
            onTop.transform.position = transform.position;
            onTop.transform.position = SetZ(onTop.transform.position, -1);
        }
    }

    public void setTop(GameObject gameObj)
    {
        onTop = gameObj;
    }
}
