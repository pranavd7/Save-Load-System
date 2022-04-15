using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;

    [SerializeField] Button saveButton;
    [SerializeField] Button loadButton;
    [SerializeField] Button deleteSaveButton;

    [SerializeField] GameObject zombearEnemyPrefab;
    [SerializeField] GameObject zombunnyEnemyPrefab;
    [SerializeField] GameObject hellephantEnemyPrefab;

    string saveFileName = "savedata";

    private void Start()
    {
        // if not assigned, find playerhealth script
        if (!playerHealth) playerHealth = FindObjectOfType<PlayerHealth>();

        RefreshButtons();
    }

    /// <summary>
    /// Save current game data into a file
    /// </summary>
    public void SaveGame()
    {
        //find all enemy health scripts
        EnemyHealth[] enemyHealthArr = FindObjectsOfType<EnemyHealth>();
        List<EnemyData> enemies = new List<EnemyData>();

        foreach (EnemyHealth e in enemyHealthArr)
        {
            enemies.Add(new EnemyData(e));
        }

        GameData gameData = new GameData(ScoreManager.score, playerHealth.CurrentHealth, enemies, playerHealth.transform.position);

        SaveLoadScript.Save(gameData, saveFileName);

        RefreshButtons();
    }

    /// <summary>
    /// Clear map of all enemies and load the saved file
    /// </summary>
    public void LoadGame()
    {
        //find all enemy health scripts and clear/destroy all enemies
        EnemyHealth[] enemyHealthArr = FindObjectsOfType<EnemyHealth>();
        foreach (EnemyHealth e in enemyHealthArr)
        {
            Destroy(e.gameObject);
        }

        GameData gameData = (GameData)SaveLoadScript.Load(saveFileName);

        playerHealth.CurrentHealth = gameData.playerHealth;
        ScoreManager.score = gameData.gameScore;
        playerHealth.transform.position = gameData.playerPosition;

        // instantiate all saved enemy according to type/name and set health to saved value
        for (int i = 0; i < gameData.enemyDataList.Count; i++)
        {
            EnemyData data = gameData.enemyDataList[i];
            EnemyHealth enemyHealth;

            switch (data.enemyType)
            {
                case "Zombear":
                    enemyHealth = Instantiate(zombearEnemyPrefab, data.position, data.rotation).GetComponent<EnemyHealth>();
                    enemyHealth.CurrentHealth = data.health;
                    break;
                case "Zombunny":
                    enemyHealth = Instantiate(zombunnyEnemyPrefab, data.position, data.rotation).GetComponent<EnemyHealth>();
                    enemyHealth.CurrentHealth = data.health;
                    break;
                case "Hellephant":
                    enemyHealth = Instantiate(hellephantEnemyPrefab, data.position, data.rotation).GetComponent<EnemyHealth>();
                    enemyHealth.CurrentHealth = data.health;
                    break;
            }
        }
    }

    /// <summary>
    /// Delete the saved game file
    /// </summary>
    public void DeleteSavedGame()
    {
        SaveLoadScript.DeleteSaveGame(saveFileName);
        RefreshButtons();
    }

    /// <summary>
    /// Enable/Disable 'load' and 'delete save' buttons if a save file exists or not
    /// </summary>
    void RefreshButtons()
    {
        loadButton.interactable = SaveLoadScript.SaveExists(saveFileName);
        deleteSaveButton.interactable = SaveLoadScript.SaveExists(saveFileName);
    }
}
