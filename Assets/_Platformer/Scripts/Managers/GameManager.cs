using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : GenericSingleton<GameManager>
{
    [SerializeField] GameMenu gameMenu;
    public SceneReferences sceneRef;
    public PlayerData playerData;
    public PlayerData initData;
    public GameState gameState;
    public Dictionary<int, bool> enemyState = new Dictionary<int, bool>();
    public Dictionary<int, bool> powerUpState = new Dictionary<int, bool>();

    private bool isGameOver;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        HandleUIInput();

        if (!isGameOver)
        {
            if(sceneRef.player.CurrentState == Player.PlayerState.Dead)
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
            Debug.Log("Escape");
            gameMenu.TogglePauseMenu();
            
            if (gameMenu.GetInfoMenuIsOpen())
            {
                gameMenu.CloseInfoMenu();
            }
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
        sceneRef.player.DisableAllActions();
        sceneRef.player.PlayAnimVictory();
        gameMenu.ShowGameWinMenu();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1.0f;

        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        gameState.restartGame = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Init() 
    {
        if(gameState.newGame) 
        {
            if (sceneRef.powerupHolder != null)
                foreach (Transform child in sceneRef.powerupHolder.transform)
                {
                    if (child.gameObject.GetComponent<Powerup>())
                        powerUpState.Add(child.gameObject.GetComponent<Powerup>().ID, true);
                }

            if (sceneRef.enemiesHolder != null)
                foreach (Transform child in sceneRef.enemiesHolder.transform)
                {
                    if (child.gameObject.GetComponent<Bot>())
                        enemyState.Add(child.gameObject.GetComponent<Bot>().ID, true);
                }
            Debug.Log("new game");

            playerData.currentHealth = playerData.maxHealth;
            gameState.newGame = false;
        }

        if (gameState.restartGame)
        { 
            LoadData();
            gameState.restartGame = false;
            Debug.Log("restart game");
        }
    }

    public void AssignEnemyID() //used in editor
    {
        int i = 0;

        foreach (Transform child in sceneRef.enemiesHolder.transform)
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

        foreach (Transform child in sceneRef.powerupHolder.transform)
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
            posX = sceneRef.player.transform.position.x,
            posY = sceneRef.player.transform.position.y,
            posZ = sceneRef.player.transform.position.z,
            enemySaveState = enemyState,
            powerupSaveState = powerUpState
        };

        #if UNITY_STANDALONE
            string filePath = Application.streamingAssetsPath + "/savedata.sav";
        #endif

        #if UNITY_WEBGL
            string filePath = Application.persistentDataPath + "/savedata.sav";
        #endif

        DataSerializer.SaveJson(savedData, filePath);

        Debug.Log("Game saved.");
    }

    public void LoadData()
    {
        SaveData savedData = new SaveData();

        #if UNITY_STANDALONE
            string filePath = Application.streamingAssetsPath + "/savedata.sav";
        #endif

        #if UNITY_WEBGL
            string filePath = Application.persistentDataPath + "/savedata.sav";
        #endif

        savedData = DataSerializer.LoadJson(filePath);

        UpdateGameState(savedData);

        Debug.Log("Game loaded.");
    }

    private void UpdateGameState(SaveData saveData)
    {
        playerData.currentHealth = saveData.currentHealth;

        sceneRef.player.transform.position = new Vector3(saveData.posX, saveData.posY, saveData.posZ);
        Debug.Log(sceneRef.player.transform.position);
        enemyState = saveData.enemySaveState;
        powerUpState = saveData.powerupSaveState;

        if (sceneRef.powerupHolder != null)
            foreach (Transform child in sceneRef.powerupHolder.transform)
            {
                child.gameObject.SetActive(powerUpState[child.gameObject.GetComponent<Powerup>().ID]);
            }

        if (sceneRef.enemiesHolder != null)
            foreach (Transform child in sceneRef.enemiesHolder.transform)
            {
                child.gameObject.SetActive(enemyState[child.gameObject.GetComponent<Bot>().ID]);
            }
    }
}
