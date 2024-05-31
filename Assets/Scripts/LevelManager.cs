using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;

    [SerializeField] Levels levels;
    [SerializeField] DifficultyLevels difficulties;
    [SerializeField] Weapons weapons;
    [SerializeField] Enemies enemies;
    
    public List<EnemyBehaviour> currentEnemies = new List<EnemyBehaviour>();

    public Difficulty currentDifficulty;
    public Level currentLevel;

    float spawnTimer = 0f;


    void Update()
    {
        if (spawnTimer < currentLevel.spawnTime)
        {
            spawnTimer += Time.deltaTime;
        }
        else
        {
            spawnTimer = 0f;

            Vector2 spawnArea = currentLevel.spawnArea;

            for (int i = 0; i < currentLevel.enemyPerSpawn; i++)
            {
                Vector3 spawnPoint = currentLevel.spawnPoint + new Vector3(Random.Range(-spawnArea.x, spawnArea.x), 10f, Random.Range(-spawnArea.y, spawnArea.y));


            }
        }
    }

    public void SpawnEnemy(Vector3 spawnPoint)
    {

    }

    public void GenerateLevel(int levelID, int difficultyID)
    {
        FlushLevel();

        currentLevel = levels.levels.First(x => x.id == levelID);
        currentDifficulty = difficulties.difficultyLevels.First(x => x.id == difficultyID);

        PlayerController.Instance.ResetPlayer();
        PlayerController.Instance.transform.position = currentLevel.spawnPoint;
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
}