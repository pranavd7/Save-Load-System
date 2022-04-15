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

    public void SaveGame()
    {
        //find all enemy health scripts
        EnemyHealth[] enemyHealthArr = FindObjectsOfType<EnemyHealth>();
        List<EnemyData> enemies = new List<EnemyData>();

        foreach (EnemyHealth e in enemyHealthArr)
        {
            enemies.Add(new EnemyData(e));
        }

        GameData gameData = new GameData(ScoreManager.score, playerHealth.CurrentHealth, enemies);

        SaveLoadScript.Save(gameData, saveFileName);

        RefreshButtons();
    }

    public void LoadGame()
    {
        //find all enemy health scripts
        EnemyHealth[] enemyHealthArr = FindObjectsOfType<EnemyHealth>();
        foreach (EnemyHealth e in enemyHealthArr)
        {
            Destroy(e.gameObject);
        }

        GameData gameData = (GameData)SaveLoadScript.Load(saveFileName);

        playerHealth.CurrentHealth = gameData.playerHealth;
        ScoreManager.score = gameData.gameScore;
        for (int i = 0; i < gameData.enemyDataList.Count; i++)
        {
            EnemyData data = gameData.enemyDataList[i];
            EnemyHealth enemyHealth;

            // instantiate enemy according to type/name and set health to saved value
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

    public void DeleteSavedGame()
    {
        SaveLoadScript.DeleteSaveGame(saveFileName);
        RefreshButtons();
    }

    /// <summary>
    /// Enable/Disable load and delete save buttons if a save file exists or not
    /// </summary>
    void RefreshButtons()
    {
        Debug.Log("save exists:" + SaveLoadScript.SaveExists(saveFileName));
        loadButton.interactable = SaveLoadScript.SaveExists(saveFileName);
        deleteSaveButton.interactable = SaveLoadScript.SaveExists(saveFileName);
    }
}
