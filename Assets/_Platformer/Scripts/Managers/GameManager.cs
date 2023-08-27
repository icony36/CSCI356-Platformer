using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : GenericSingleton<GameManager>
{
    [SerializeField] GameMenu gameMenu;
    
    public Player Player { get; private set; }

    public PlayerData playerData;
    public PlayerData initData;
    public Dictionary<int, bool> enemyState = new Dictionary<int, bool>();
    public Dictionary<int, bool> powerUpState = new Dictionary<int, bool>();

    [SerializeField] private GameObject enemiesHolder;
    [SerializeField] private GameObject powerupHolder;

    private bool isGameOver;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();           

        Init();
    }

    private void Update()
    {
        HandleUIInput();

        if (!isGameOver)
        {
            if(Player.CurrentState == Player.PlayerState.Dead)
            {
                isGameOver = true;
                GameIsOver();
            }
        }
    }

    private void HandleUIInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameMenu.TogglePauseMenu();
        }
    }

    private void GameIsOver()
    {
        Debug.Log("GAME OVER");
        gameMenu.ShowGameOverMenu();
    }

    public void GameIsFinished()
    {
        Debug.Log("GAME FINISHED");
        Player.DisableAllActions();
        Player.PlayAnimVictory();
        gameMenu.ShowGameWinMenu();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1.0f;

        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        //gameMenu.TogglePauseMenu();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        LoadData();
    }

    public void Init() //need this to only be called on new game start
    {
        if(powerupHolder != null)
            foreach (Transform child in powerupHolder.transform)
            {
                if (child.gameObject.GetComponent<Powerup>())
                    powerUpState.Add(child.gameObject.GetComponent<Powerup>().ID, true);
            }

        if(enemiesHolder != null)
            foreach (Transform child in enemiesHolder.transform)
            {
                if (child.gameObject.GetComponent<Bot>())
                    enemyState.Add(child.gameObject.GetComponent<Bot>().ID, true);
            }
    }

    public void AssignEnemyID() //used in editor
    {
        int i = 0;

        foreach (Transform child in enemiesHolder.transform)
        {
            child.gameObject.GetComponent<Bot>().ID = i;
            enemyState.Add(child.gameObject.GetComponent<Bot>().ID, false);


            #if UNITY_EDITOR
                EditorUtility.SetDirty(child.gameObject.GetComponent<Bot>());
            #endif

            i++;
        }
    }

    public void AssignPowerupID() //used in editor
    {
        int i = 0;

        foreach (Transform child in powerupHolder.transform)
        {
            child.gameObject.GetComponent<Powerup>().ID = i;
            powerUpState.Add(child.gameObject.GetComponent<Powerup>().ID, false);

            #if UNITY_EDITOR
                EditorUtility.SetDirty(child.gameObject.GetComponent<Powerup>());
            #endif

            i++;
        }
    }

    public void SaveData()
    {
        SaveData savedData = new SaveData
        {
            currentHealth = playerData.currentHealth,
            posX = Player.transform.position.x,
            posY = Player.transform.position.y,
            posZ = Player.transform.position.z,
            enemySaveState = enemyState,
            powerupSaveState = powerUpState
        };

        string filePath = Application.streamingAssetsPath + "/savedata.sav";

        DataSerializer.SaveJson(savedData, filePath);

        Debug.Log("Game saved.");
    }

    public void LoadData()
    {
        SaveData savedData = new SaveData();

        string filePath = Application.streamingAssetsPath + "/savedata.sav";

        savedData = DataSerializer.LoadJson(filePath);

        UpdateGameState(savedData);

        Debug.Log("Game loaded.");
    }

    private void UpdateGameState(SaveData saveData)
    {
        playerData.currentHealth = saveData.currentHealth;

        Player.transform.position = new Vector3(saveData.posX, saveData.posY, saveData.posZ);

        enemyState = saveData.enemySaveState;
        powerUpState = saveData.powerupSaveState;

        if (powerupHolder != null)
            foreach (Transform child in powerupHolder.transform)
            {
                child.gameObject.SetActive(powerUpState[child.gameObject.GetComponent<Powerup>().ID]);
            }

        if (enemiesHolder != null)
            foreach (Transform child in enemiesHolder.transform)
            {
                child.gameObject.SetActive(enemyState[child.gameObject.GetComponent<Bot>().ID]);
            }
    }
}
