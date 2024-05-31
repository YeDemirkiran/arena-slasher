using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

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

    void Start()
    {
        currentLevel = levels.levels[0];
        currentDifficulty = difficulties.difficultyLevels[0];
    }

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
                SpawnEnemy(spawnPoint);
            }
        }
    }

    public void SpawnEnemy(Vector3 spawnPoint)
    {
        int id = currentLevel.enemyIDs[Random.Range(0, currentLevel.enemyIDs.Length)];
        Enemy enemy = enemies.enemies.First(x => x.id == id);

        GameObject enemyObj = Instantiate(enemy.prefab, spawnPoint, Quaternion.identity);
        EnemyBehaviour enemyBehaviour = enemyObj.GetComponent<EnemyBehaviour>();
        BotOutfit outfit = enemyObj.GetComponent<BotOutfit>();

        outfit.SetHeadGear(enemy.headGear);
        outfit.SetTorsoGear(enemy.torsoGear);
        outfit.SetPantsGear(enemy.pantsGear);
        outfit.SetFeetGear(enemy.feetGear);

        Weapon weapon = weapons.weapons.First(x => x.id == enemy.weaponID);
        outfit.SetWeapon(weapon);
        currentEnemies.Add(enemyBehaviour);
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