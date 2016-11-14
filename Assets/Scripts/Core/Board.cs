using System;
using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour
{

    public Transform m_EmptySprite;
    public int m_Height = 30;
    public int m_Width = 10;
    public int m_Header = 8;

	// Use this for initialization
	void Start () {
	    DrawEmptyCells();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void DrawEmptyCells()
    {
        if (m_EmptySprite != null)
        {
            for (var y = 0; y < m_Height - m_Header; y++)
            {
                for (var x = 0; x < m_Width; x++)
                {
                    var clone = Instantiate(m_EmptySprite, new Vector3(x, y, 0), Quaternion.identity) as Transform;
                    if (clone == null) continue;

                    clone.name = string.Format("Board Space ( x = {0} , y = {1}", x, y);
                    clone.transform.parent = transform;
                }
            }
        }
        else
        {
            Debug.Log("Warning no sprite!");
        }
    }
}
