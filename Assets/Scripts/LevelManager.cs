using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] GameObject playerPrefab;

    [SerializeField] Levels levels;
    [SerializeField] DifficultyLevels difficulties;
    [SerializeField] Weapons weapons;
    [SerializeField] Enemies enemies;
    
    public List<EnemyBehaviour> currentEnemies { get; private set; } =  new List<EnemyBehaviour>();

    public Difficulty currentDifficulty { get; private set; }
    public Level currentLevel { get; private set; }

    float spawnTimer = 0f;

    public float levelTimer { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    void Start()
    {
        GenerateLevel(0, 3);
    }

    void Update()
    {
        if (spawnTimer < currentLevel.spawnTime)
        {
            spawnTimer += Time.deltaTime;
            //Debug.Log("1");

        }
        else
        {
            spawnTimer = 0f;

            //Debug.Log("!2");

            Vector2 spawnArea = currentLevel.spawnArea;
            int availableEnemy = currentDifficulty.maxEnemies - currentEnemies.Count;

            //Debug.Log(currentDifficulty.maxEnemies);

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
                // Level over
            }
        }
        else
        {
            levelTimer += Time.deltaTime;

            // write score on death
        }
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

        BotOutfit outfit = enemyObj.GetComponent<BotOutfit>();

        //outfit.SetHeadGear(enemy.headGear);
        outfit.SetTorsoGear(enemy.torsoGear);
        //outfit.SetPantsGear(enemy.pantsGear);
        //outfit.SetFeetGear(enemy.feetGear);

        //Weapon weapon = weapons.weapons.First(x => x.id == enemy.weaponID);
        //outfit.SetWeapon(weapon);
        currentEnemies.Add(enemyBehaviour);

        Debug.Log("Spawned enemy");
    }

    public void GenerateLevel(int levelID, int difficultyID)
    {
        FlushLevel();

        currentLevel = levels.levels.First(x => x.id == levelID);
        currentDifficulty = difficulties.difficultyLevels.First(x => x.id == difficultyID);

        //PlayerController.Instance.ResetPlayer();
        //PlayerController.Instance.transform.position = currentLevel.spawnPoint;

        if (currentLevel.type == Level.LevelType.Timed)
        {
            levelTimer = currentLevel.duration;
        }
        else
        {
            levelTimer = 0f;
        }

        GenerateArena();
    }

    void FlushLevel()
    {
        for (int i = currentEnemies.Count - 1; i >= 0; i--)
        {
            Destroy(currentEnemies[i].gameObject);
        }

        currentEnemies.Clear();

        spawnTimer = 0f;

        //Destroy(PlayerController.Instance.gameObject);  
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

        //Vector3 groundScale = ground.transform.localScale;
        //groundScale.x = area.x;
        //groundScale.z = area.y;
        //ground.transform.
    }

    public void EnemyDeathReport(EnemyBehaviour enemy)
    {
        currentEnemies.Remove(enemy);
    }
}