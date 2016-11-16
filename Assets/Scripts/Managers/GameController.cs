using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    private Board _mGameBoard;

    private Spawner _mSpawner;

    private Shape _mActiveShape;

    private float _timetoDrop;
    private float _dropInterval = .9f;

    private bool _mGameOver = false;

    [Range(.02f, 1f)]
    public float KeyRepeatRateLeftRight = 0.25f;
    private float _timeToNextKeyLeftRight;

    [Range(.02f, 1f)]
    public float KeyRepeatRateDown = 0.068f;
    private float _timeToNextKeyDown;

    [Range(.02f, 1f)]
    public float KeyRepeatRateRotate = 0.2f;
    private float _timeToNextKeyRotate;

    private SoundManager _soundManager;

    public GameObject GameOverPanel;

    void PlayerInput()
    {
        if((Input.GetButton("MoveRight") && (Time.time > _timeToNextKeyLeftRight)) || Input.GetButtonDown("MoveRight"))
        {
            _mActiveShape.MoveRight();
            _timeToNextKeyLeftRight = Time.time + KeyRepeatRateLeftRight;

            if (!_mGameBoard.IsValidPosition(_mActiveShape))
            {
                _mActiveShape.MoveLeft();
            }
        } else

        if ((Input.GetButton("MoveLeft") && (Time.time > _timeToNextKeyLeftRight)) || Input.GetButtonDown("MoveLeft"))
        {
            _mActiveShape.MoveLeft();
            _timeToNextKeyLeftRight = Time.time + KeyRepeatRateLeftRight;
             
            if (!_mGameBoard.IsValidPosition(_mActiveShape))
            {
                _mActiveShape.MoveRight();
            }
        } else

        if (Input.GetButtonDown("Rotate") && (Time.time > _timeToNextKeyRotate))
        {
            _mActiveShape.RotateRight();
            _timeToNextKeyRotate = Time.time + KeyRepeatRateRotate;

            if (!_mGameBoard.IsValidPosition(_mActiveShape))
            {
                _mActiveShape.RotateLeft();
            }
        } else if ((Input.GetButton("MoveDown") && (Time.time > _timeToNextKeyDown)) || (Time.time > _timetoDrop))
        {
            _timetoDrop = Time.time + _dropInterval;
            _timeToNextKeyDown = Time.time + KeyRepeatRateDown;
            _mActiveShape.MoveDown();

            if (!_mGameBoard.IsValidPosition(_mActiveShape))
            {
                if (_mGameBoard.IsOverLimit(_mActiveShape))
                {
                    GameOver();
                }
                else
                {
                    LandShape();
                }
            }
        }

    }

    private void GameOver()
    {
        _mActiveShape.MoveUp();
        _mGameOver = true;

        if (GameOverPanel)
        {
            GameOverPanel.SetActive(true);
        }
        Debug.LogWarning(_mActiveShape.name + " is over the limit");
    }

    public void Restart()
    {
        Debug.Log("Restarted");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LandShape()
    {
        _timeToNextKeyDown = Time.time;
        _timeToNextKeyLeftRight = Time.time;
        _timeToNextKeyRotate = Time.time;

        _mActiveShape.MoveUp();
        _mGameBoard.StoreShape(_mActiveShape);
        _mActiveShape = _mSpawner.SpawnShape();

        _mGameBoard.ClearAllRows();
    }

    // Use this for initialization
    void Start()
    {
        _mGameBoard = GameObject.FindObjectOfType<Board>();
        _mSpawner = GameObject.FindObjectOfType<Spawner>();
        _soundManager = GameObject.FindObjectOfType<SoundManager>();

        _timeToNextKeyDown = Time.time + KeyRepeatRateDown;
        _timeToNextKeyLeftRight = Time.time + KeyRepeatRateLeftRight;
        _timeToNextKeyRotate = Time.time + KeyRepeatRateRotate;

        if (!_mGameBoard)
        {
            Debug.LogWarning(("WARNING! There is no game board defined!"));
        }

        if (!_soundManager)
        {
            Debug.LogWarning("WARNING! There is no sound manager defined!");
        }

        if (!_mSpawner)
        {
            Debug.LogWarning("WARNING! There is no spawner defined!");
        }
        else
        {
            _mSpawner.transform.position = Vectorf.Round(_mSpawner.transform.position);

            if (!_mActiveShape)
            {
                _mActiveShape = _mSpawner.SpawnShape();
            }
        }

        if (GameOverPanel)
        {
            GameOverPanel.SetActive(false);
        }

        if (_soundManager && _soundManager.m_moveSound && _soundManager.m_fxEnabled)
        {

            AudioSource.PlayClipAtPoint(_soundManager.m_moveSound, Camera.main.transform.position,
                _soundManager.m_fxVolume);
        }
    }

    // Update is called once per frame
	void Update ()
	{
	    if (!_mGameBoard || !_mSpawner || !_mActiveShape || _mGameOver || !_soundManager)
	    {
	        return;
	    }
        
        PlayerInput();
	}
}
