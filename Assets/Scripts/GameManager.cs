using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
    
    public int enemiesAlive;

    public int round = 0;
    
    public GameObject[] spawnPoints;

    public GameObject enemyPrefab;

    public Image HpBar;
    public TextMeshProUGUI RoundText;
    public TextMeshProUGUI RoundsSurvivedText;
    
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    public bool isPaused;
    public bool isGameOver;
    
    public PhotonView photonView;
    
    void Start()
    {
        isPaused = false;
        isGameOver = false;
        Time.timeScale = 1;
        
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawner");
    }

    // Update is called once per frame
    void Update()
    {

        if ( !PhotonNetwork.InRoom || ( PhotonNetwork.IsMasterClient && photonView.IsMine ) )
        {
            if (enemiesAlive <= 0)
            {
                NextWave();
            }
        }
        

        if (Input.GetKeyDown(KeyCode.Escape) && !gameOverPanel.activeSelf)
        {
            if (pausePanel.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        
    }

    public void NextWave()
    {
        int min = 0;
        int max = spawnPoints.Length - 1;

        round++;
        if (PhotonNetwork.InRoom)
        {
            Hashtable hash = new Hashtable();
            hash.Add("RoundNumber", round);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        else
        {
            DisplayNextRound( round.ToString() );    
        }
        
        for (int i = 0; i < round; i++)
        {
            int index = Random.Range(min, max);
        
            GameObject selectedSpawn = spawnPoints[index];

            GameObject zombi;
            if (PhotonNetwork.InRoom)
            {
                zombi = PhotonNetwork.Instantiate("Zombie", selectedSpawn.transform.position, Quaternion.identity);;
            }
            else
            {
                zombi = Instantiate(enemyPrefab, selectedSpawn.transform.position, Quaternion.identity);
            }
            zombi.GetComponent<EnemyManager>().gameManager = this;
            enemiesAlive++;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
        
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        RoundsSurvivedText.text = (round - 1).ToString();
        
        isGameOver = true;
    }

    private void DisplayNextRound(string roundNumber)
    {
        RoundText.text = roundNumber;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (photonView.IsMine)
        {
            if (changedProps["RoundNumber"] != null)
            {
                DisplayNextRound( (string) changedProps["RoundNumber"] );
            }
        }
    }
}
