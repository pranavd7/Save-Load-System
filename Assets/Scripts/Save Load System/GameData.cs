using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public string enemyType; // enemy name since there are no other way to differntiate
    public int health;
    public Vector3 position;
    public Quaternion rotation;

    public EnemyData(EnemyHealth enemy)
    {
        enemyType = enemy.transform.name.Split('(')[0];
        health = enemy.CurrentHealth;
        position = enemy.transform.position;
        rotation = enemy.transform.rotation;
    }
}

[System.Serializable]
public class GameData
{
    public List<EnemyData> enemyDataList;
    public int gameScore;
    public int playerHealth;

    public GameData(int score, int health, List<EnemyData> enemies)
    {
        enemyDataList = enemies;
        gameScore = score;
        playerHealth = health;
    }

}
