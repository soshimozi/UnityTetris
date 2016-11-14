using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    private Board _mGameBoard;

    private Spawner _mSpawner;

    private Shape _mActiveShape;

	// Use this for initialization
	void Start ()
	{
	    _mGameBoard = GameObject.FindObjectOfType<Board>();
	    _mSpawner = GameObject.FindObjectOfType<Spawner>();

	    if (_mSpawner)
	    {
	        if (_mActiveShape == null)
	        {
	            _mActiveShape = _mSpawner.SpawnShape();
	        }

	        _mSpawner.transform.position = Vectorf.Round(_mSpawner.transform.position);
	    }
	    if (!_mGameBoard)
	    {
	        Debug.LogWarning(("WARNING! There is no game board defined!"));
	    }

	    if (!_mSpawner)
	    {
	        Debug.LogWarning("WARNING! There is no spawner defined!");
	    }
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (!_mGameBoard || !_mSpawner)
	    {
	        return;
	    }

	    if (_mActiveShape)
	    {
	        _mActiveShape.MoveDown();
	    }
	}
}
