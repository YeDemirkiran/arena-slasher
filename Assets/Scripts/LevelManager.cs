using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
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

    void Start()
    {
        GenerateLevel(0, 3);
    }

    void Update()
    {
        if (spawnTimer < currentLevel.spawnTime)
        {
            spawnTimer += Time.deltaTime;
            Debug.Log("1");

        }
        else
        {
            spawnTimer = 0f;

            Debug.Log("!2");

            Vector2 spawnArea = currentLevel.spawnArea;
            int availableEnemy = currentDifficulty.maxEnemies - currentEnemies.Count;

            Debug.Log(currentDifficulty.maxEnemies);

            for (int i = 0; i < Mathf.Clamp(currentLevel.enemyPerSpawn, 0, availableEnemy); i++)
            {
                Vector3 spawnPoint = currentLevel.spawnPoint + new Vector3(Random.Range(-spawnArea.x, spawnArea.x), 10f, Random.Range(-spawnArea.y, spawnArea.y));
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
            levelTimer = currentDifficulty.duration;
        }
        else
        {
            levelTimer = 0f;
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

        //Destroy(PlayerController.Instance.gameObject);  
    }

    public void EnemyDeathReport(EnemyBehaviour enemy)
    {
        currentEnemies.Remove(enemy);
    }
}