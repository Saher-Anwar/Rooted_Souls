using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : EnemyAI
{

    [SerializeField]
    private List<EnemyAI> differentTypesOfEnemies = new List<EnemyAI>();
    [SerializeField]
    float enemySpawnDelay = 1.5f;

    LevelManager levelManager;
    float elapsedtime = 0;

    public BossEnemy(LevelManager levelManager, Transform spawnLocation)
    {
        this.levelManager = levelManager;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedtime += Time.deltaTime;

        if(elapsedtime > enemySpawnDelay)
        {
            EnemyAI randomEnemy = differentTypesOfEnemies[Random.Range(0, differentTypesOfEnemies.Count)];
            Instantiate(randomEnemy, transform.position, Quaternion.identity);
            elapsedtime = 0;
        }
    }
}
