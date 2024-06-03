using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState { Running, Paused }

    GameState _state;
    public GameState state { get { return _state; } private set { _state = value; EvaluateGameState(); } }

    void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;

        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        state = GameState.Paused;
    }

    public void SetMouse(bool on)
    {
        Cursor.visible = on;
        Cursor.lockState = on ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void CreateSession(int levelId, int difficultyId)
    {
        StopAllCoroutines();
        StartCoroutine(CreateSessionIE(levelId, difficultyId));
    }

    IEnumerator CreateSessionIE(int levelId, int difficultyId)
    {
        state = GameState.Running;
        SceneManager.LoadScene("Game");

        while (LevelManager.Instance == null)
        {
            yield return null;
        }

        LevelManager.Instance.GenerateLevel(levelId, difficultyId);
    }

    void EvaluateGameState()
    {
        if (state == GameState.Running)
        {
            SetMouse(false);
        }
        else if (state == GameState.Paused)
        {
            SetMouse(true);
        }
    }
}