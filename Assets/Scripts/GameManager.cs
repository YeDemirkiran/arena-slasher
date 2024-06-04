using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct GameData
{
    // Unlocked
    public int[] unlockedLevelIDs;
    public int[] unlockedGearIDs;
    public int[] unlockedWeaponIDs;

    // Options
    public float sfxLevel;
    public float musicLevel;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState { Running, Paused, MainMenu }

    GameState _state;
    public GameState state { get { return _state; } private set { _state = value; EvaluateGameState(); } }

    public UnityAction onResume, onPause, onGameBegin, onMainMenu;

    [HideInInspector] public GameData gameData;
    [SerializeField] DefaultGameData defaultGameData;

    void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;

        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        LoadDataFromFile(Application.persistentDataPath + "/data.bin");
    }

    void Start()
    {
        state = GameState.MainMenu;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == GameState.Paused)
            {
                ContinueGame();
            }
            else if (state == GameState.Running)
            {
                PauseGame();
            }
        }
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

    public void PauseGame()
    {
        state = GameState.Paused;
        onPause?.Invoke();
    }

    public void PauseGamePure()
    {
        state = GameState.Paused;
    }

    public void ContinueGame()
    {
        state = GameState.Running;
        onResume?.Invoke();
    }

    public void GoToMainMenu()
    {
        state = GameState.MainMenu;
        SceneManager.LoadScene("Menu");
        onMainMenu?.Invoke();
        DestroyImmediate(gameObject);
    }

    IEnumerator CreateSessionIE(int levelId, int difficultyId)
    {
        SceneManager.LoadScene("Game");

        while (LevelManager.Instance == null)
        {
            yield return null;
        }

        state = GameState.Running;
        LevelManager.Instance.GenerateLevel(levelId, difficultyId);
    }

    void EvaluateGameState()
    {
        if (state == GameState.Running)
        {
            Time.timeScale = 1f;
            SetMouse(false);
        }
        else if (state == GameState.Paused || state == GameState.MainMenu)
        {
            Time.timeScale = 0f;
            SetMouse(true);
        }
    }

    public void ExitGame()
    {
        SaveGame();
        Application.Quit();
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        SaveGameToFile(Application.persistentDataPath + "/data.bin");
    }

    void SaveGameToFile(string filePath)
    {
        if (!File.Exists(filePath))
            File.Delete(filePath);

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream saveFile = File.Create(filePath);
        formatter.Serialize(saveFile, gameData);
        saveFile.Close();
    }

    void LoadDataFromFile(string filePath)
    {
        GameData data;

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);
            data = (GameData)formatter.Deserialize(file);
            file.Close();
        }
        else
        {
            data = defaultGameData.gameData;
        }

        gameData = data;
    }
}