using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{

    public Shape[] m_allShapes;

	// Use this for initialization
	void Start ()
	{
	    var ov = new Vector2(-23.3f, -12.22f);
	    var nv = Vectorf.Round(ov);

        Debug.Log(nv.ToString());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    Shape GetRandomShape()
    {
        int i = Random.Range(0, m_allShapes.Length);
        if (m_allShapes[i])
        {
            return m_allShapes[i];
        }
        else
        {
            Debug.Log("WARNING! Invalid shape!");
            return null;
        }
    }

    public Shape SpawnShape()
    {
        Shape shape = null;
        shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
        if (shape)
        {
            return shape;
        }
        else
        {
            Debug.LogWarning("WARNING! Invalid shape in spawner!");
            return null;
        }
    }
}
