using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Player player;
    private GameObject[] enemies;
    private GameObject[] powerUps;
    private bool isGameOver;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        powerUps = GameObject.FindGameObjectsWithTag("PowerUp");
    }

    private void Update()
    {
        if (!isGameOver)
        {
            if(player.CurrentState == Player.PlayerState.Dead)
            {
                isGameOver = true;
                GameIsOver();
            }
        }
    }

    private void GameIsOver()
    {
        Debug.Log("GAME OVER");
    }

    public void GameIsFinished()
    {
        Debug.Log("GAME FINISHED");
        player.DisableAllActions();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1.0f;

        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SaveGame()
    {
        //PlayerPrefs.SetFloat("chekPointPosX", player.LastCheckPoint.transform.position.x);
        //PlayerPrefs.SetFloat("chekPointPosY", player.LastCheckPoint.transform.position.y);
        //PlayerPrefs.SetFloat("chekPointPosZ", player.LastCheckPoint.transform.position.z);

        //PlayerPrefs.SetFloat("chekPointRotX", player.LastCheckPoint.transform.rotation.x);
        //PlayerPrefs.SetFloat("chekPointRotY", player.LastCheckPoint.transform.rotation.y);
        //PlayerPrefs.SetFloat("chekPointRotZ", player.LastCheckPoint.transform.rotation.z);

        Debug.Log("Game saved");
    }

    public void LoadGame()
    {

    }
}
