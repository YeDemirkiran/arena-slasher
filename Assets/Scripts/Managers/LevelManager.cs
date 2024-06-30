using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] GameObject playerPrefab;

    public Levels levels;
    [SerializeField] DifficultyLevels difficulties;
    [SerializeField] Enemies enemies;

    [SerializeField] Volume sceneVolume;

    [SerializeField] GameObject winMenu;

    public Difficulty currentDifficulty { get; private set; }
    public Level currentLevel { get; private set; }
    public List<EnemyBehaviour> currentEnemies { get; private set; } = new List<EnemyBehaviour>();


    float spawnTimer = 0f;

    public float levelTimer { get; private set; }
    bool levelCreated = false;

    List<GameObject> arenaObjects = new List<GameObject>();

    void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    void Update()
    {
        if (GameManager.Instance.state != GameManager.GameState.Running) { return; }
        if (!levelCreated) return;

        if (spawnTimer < currentLevel.spawnTime && currentEnemies.Count > 0)
        {
            spawnTimer += Time.deltaTime;
        }
        else
        {
            spawnTimer = 0f;

            Vector2 spawnArea = currentLevel.spawnArea;
            int availableEnemy = currentDifficulty.maxEnemies - currentEnemies.Count;

            for (int i = 0; i < Mathf.Clamp(currentLevel.enemyPerSpawn, 0, availableEnemy); i++)
            {
                Vector3 spawnPoint = currentLevel.spawnPoint + new Vector3(Random.Range(-spawnArea.x / 2f, spawnArea.x / 2f), 0f, Random.Range(-spawnArea.y / 2f, spawnArea.y / 2f));
                SpawnEnemy(spawnPoint);
            }
        }

        if (currentLevel.type == Level.LevelType.Timed)
        {
            levelTimer -= Time.deltaTime;

            if (levelTimer <= 0f)
            {
                GameManager.Instance.PauseGamePure();
                winMenu.SetActive(true);
            }
        }
        else
        {
            levelTimer += Time.deltaTime;

            // write score on death
        }
    }

    public void RestartLevel()
    {
        GenerateLevel(currentLevel.id, currentDifficulty.id);
    }

    public void SpawnPlayer()
    {
        PlayerController player = Instantiate(playerPrefab).GetComponent<PlayerController>();
        CharacterController cha = player.GetComponent<CharacterController>();
        cha.enabled = false;

        BotOutfit outfit = player.GetComponent<BotOutfit>();

        foreach (var item in GameManager.Instance.gameData.equippedItemIDs)
        {
            Item gear = Items.Instance[item];

            outfit.SetGear(gear.prefab, outfit.transform);
        }

        Weapon weapon = Items.Instance.weapons[GameManager.Instance.gameData.equippedWeaponID];
        player.controller.currentWeapon = weapon;
        player.controller.weaponControllers = outfit.SetWeapon(weapon.prefab, outfit.transform);
        player.controller.animator.runtimeAnimatorController = weapon.controller;

        player.transform.position = currentLevel.spawnPoint;
        cha.enabled = true;
    }

    public void SpawnEnemy(Vector3 spawnPoint, int enemyID = -1)
    {
        int id;
          
        if (enemyID == -1) { id = currentLevel.enemyIDs[Random.Range(0, currentLevel.enemyIDs.Length)]; }
        else { id = enemyID; }
        
        Enemy enemy = enemies.enemies.First(x => x.id == id);

        GameObject enemyObj = Instantiate(enemy.prefab, spawnPoint, Quaternion.identity);

        EnemyBehaviour enemyBehaviour = enemyObj.GetComponent<EnemyBehaviour>();
        enemyBehaviour.level = this;
        enemyBehaviour.drops = enemy.drops;

        currentEnemies.Add(enemyBehaviour);

        BotOutfit outfit = enemyObj.GetComponent<BotOutfit>();

        foreach (var gear in enemy.items)
        {
            outfit.SetGear(gear.prefab, outfit.transform);
        }

        enemyBehaviour.controller.currentWeapon = enemy.weapon;
        enemyBehaviour.controller.weaponControllers = outfit.SetWeapon(enemy.weapon.prefab, outfit.transform);
        enemyBehaviour.controller.animator.runtimeAnimatorController = enemy.weapon.controller;
    }

    public void GenerateLevel(int levelID, int difficultyID)
    {
        FlushLevel();

        currentLevel = levels.levels.First(x => x.id == levelID);
        currentDifficulty = difficulties.difficultyLevels.First(x => x.id == difficultyID);

        SpawnPlayer();

        if (currentLevel.type == Level.LevelType.Timed)
        {
            levelTimer = currentDifficulty.duration;
        }
        else
        {
            levelTimer = 0f;
        }

        GenerateArena();

        sceneVolume.profile = currentLevel.volume;

        levelCreated = true;
        spawnTimer = currentLevel.spawnTime;

        if (AudioManager.Instance.musicSource.clip != currentLevel.soundtrack)
        {
            AudioManager.Instance.musicSource.clip = currentLevel.soundtrack;
            AudioManager.Instance.musicSource.Play();
        }
    }

    void FlushLevel()
    {
        for (int i = currentEnemies.Count - 1; i >= 0; i--)
        {
            Destroy(currentEnemies[i].gameObject);
        }

        currentEnemies.Clear();

        spawnTimer = 0f;

        foreach (var obj in arenaObjects)
        {
            Destroy(obj);
        }

        arenaObjects.Clear();

        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.ResetPlayer();
            DestroyImmediate(PlayerController.Instance.gameObject);
        }
    }

    void GenerateArena()
    {
        Vector2 area = currentLevel.arenaArea;

        Transform parent = new GameObject("Arena").transform;
        parent.position = currentLevel.center;

        GameObject ground = Instantiate(currentLevel.ground, parent);

        GameObject topWall = Instantiate(currentLevel.wall, parent);
        GameObject rightWall = Instantiate(currentLevel.wall, parent);
        GameObject leftWall = Instantiate(currentLevel.wall, parent);
        GameObject bottomWall = Instantiate(currentLevel.wall, parent);

        GameObject leftTopColumn = Instantiate(currentLevel.column, parent);
        GameObject rightTopColumn = Instantiate(currentLevel.column, parent);
        GameObject leftBottomColumn = Instantiate(currentLevel.column, parent);
        GameObject rightBottomColumn = Instantiate(currentLevel.column, parent);

        ground.transform.localPosition = Vector3.zero;

        leftTopColumn.transform.localPosition = Vector3.zero + new Vector3(-area.x, 0f, area.y) / 2f;
        rightTopColumn.transform.localPosition = Vector3.zero + new Vector3(area.x, 0f, area.y) / 2f;
        leftBottomColumn.transform.localPosition = Vector3.zero + new Vector3(-area.x, 0f, -area.y) / 2f;
        rightBottomColumn.transform.localPosition = Vector3.zero + new Vector3(area.x, 0f, -area.y) / 2f;

        leftTopColumn.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
        rightTopColumn.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        leftBottomColumn.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        rightBottomColumn.transform.localEulerAngles = new Vector3(0f, 270f, 0f);

        topWall.transform.localPosition = Vector3.zero + new Vector3(0f, 0f, area.y) / 2f;
        rightWall.transform.localPosition = Vector3.zero + new Vector3(area.x, 0f, 0f) / 2f;
        leftWall.transform.localPosition = Vector3.zero + new Vector3(-area.x, 0f, 0f) / 2f;
        bottomWall.transform.localPosition = Vector3.zero + new Vector3(0f, 0f, -area.y) / 2f;

        topWall.transform.localScale = new Vector3(area.x, 0f, area.y) / 10f;
        rightWall.transform.localScale = new Vector3(area.x, 0f, area.y) / 10f;
        leftWall.transform.localScale = new Vector3(area.x, 0f, area.y) / 10f;
        bottomWall.transform.localScale = new Vector3(area.x, 0f, area.y) / 10f;

        rightWall.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
        leftWall.transform.localEulerAngles = new Vector3(0f, 0f, 90f);

        ground.GetComponent<Renderer>().material = currentLevel.groundMaterial;

        leftTopColumn.GetComponent<Renderer>().material
            = rightTopColumn.GetComponent<Renderer>().material
            = leftBottomColumn.GetComponent<Renderer>().material
            = rightBottomColumn.GetComponent<Renderer>().material
            = currentLevel.columnMaterial;

        topWall.GetComponent<Renderer>().material
            = rightWall.GetComponent<Renderer>().material
            = leftWall.GetComponent<Renderer>().material
            = bottomWall.GetComponent<Renderer>().material
            = currentLevel.wallMaterial;

        arenaObjects.Add(parent.gameObject);
        arenaObjects.Add(ground);
        arenaObjects.Add(leftWall);
        arenaObjects.Add(rightWall);
        arenaObjects.Add(topWall);
        arenaObjects.Add(bottomWall);
        arenaObjects.Add(leftTopColumn);
        arenaObjects.Add(rightTopColumn);
        arenaObjects.Add(leftBottomColumn);
        arenaObjects.Add(rightBottomColumn);

        GenerateProps();
    }

    void GenerateProps()
    {
        GameObject parent = new GameObject("Props");
        parent.transform.position = currentLevel.center;
        arenaObjects.Add(parent);

        foreach (var prop in currentLevel.props)
        {
            for (int i = 0; i < prop.count; i++)
            {
                Vector2 area = currentLevel.arenaArea;
                GameObject propObj = Instantiate(prop.prefabs[Random.Range(0, prop.prefabs.Length)]);
                propObj.transform.position = currentLevel.center + new Vector3(Random.Range(-area.x / 2f, area.x / 2f), 0f, Random.Range(-area.y / 2f, area.y / 2f));
                propObj.transform.eulerAngles = new Vector3(propObj.transform.eulerAngles.x, Random.Range(0f, 180f), propObj.transform.eulerAngles.z);
                propObj.transform.localScale = new Vector3(Random.Range(prop.sizeA.x, prop.sizeB.x), Random.Range(prop.sizeA.y, prop.sizeB.y), Random.Range(prop.sizeA.z, prop.sizeB.z));
                propObj.transform.parent = parent.transform;
                arenaObjects.Add(propObj);
            }
        }
    }

    public void EnemyDeathReport(EnemyBehaviour enemy)
    {
        currentEnemies.Remove(enemy);
    }
}