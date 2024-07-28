using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct GameData
{
    // Player Data
    public List<int> unlockedLevelIDs;

    public List<int> boughtItemIDs;
    public List<int> equippedItemIDs;

    public List<int> boughtWeaponIDs;
    public int equippedWeaponID;

    // Options
    public float sfxLevel;
    public float musicLevel;

    int _currency;
    public int currency 
    {  
        get
        {
            return _currency;
        }
        set
        {
            _currency = Mathf.Clamp(value, 0, 999999);
        }
    }

    public bool CheckLevelStatus(int id)
    {
        foreach (var level in unlockedLevelIDs)
        {
            if (level == id) return true;    
        }

        return false;
    }

    public void BuyItem(Item item)
    {
        if (boughtItemIDs.Contains(item.id) || currency < item.price)
            return;

        boughtItemIDs.Add(item.id);
        currency -= item.price;
    }

    public void EquipItem(Item item, int replacement)
    {
        if (boughtItemIDs.Contains(item.id))
        {
            boughtItemIDs.Remove(replacement);
            equippedItemIDs.Add(item.id);
        }
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState { Running, Paused, MainMenu }

    GameState _state;
    public GameState state { get { return _state; } private set { _state = value; EvaluateGameState(); } }

    float _gameSpeed = 1f;
    public float gameSpeed
    {
        get { return _gameSpeed; } private set { _gameSpeed = Mathf.Clamp(value, 0f, Mathf.Infinity); Time.timeScale = _gameSpeed; Debug.Log("GAME SPEED: " + _gameSpeed); }
    }

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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gameSpeed -= 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gameSpeed += 0.1f;
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
            Time.timeScale = gameSpeed;
            SetMouse(false);
        }
        else if (state == GameState.Paused || state == GameState.MainMenu)
        {
            Time.timeScale = 0f; // Set the timescale itself so the previous game speed is stored safely
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

    public void ResetSave()
    {
        ResetGameData();
        SaveGameToFile(Application.persistentDataPath + "/data.bin");
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    void SaveGameToFile(string filePath)
    {
        RemoveDataFile(filePath);

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

    void RemoveDataFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    void ResetGameData()
    {
        gameData = defaultGameData.gameData;
    }
}