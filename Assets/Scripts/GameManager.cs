using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;

    [SerializeField]
    public Transform nestTransform;

    [SerializeField]
    PlayerController player;
    [SerializeField]
    TheNest nest;

    public Cinemachine.CinemachineVirtualCamera mainCam;

    [SerializeField]
    DayNightCycle dayNight;

    public PlayerController Player => player;
    public TheNest Nest => nest;
    public DayNightCycle DayNight => dayNight;

    public List<Enemy> Enemies = new List<Enemy>();

    public Vector2 NestPosition => nestTransform.position;
    public Vector2 PlayerPosition => player.transform.position;

    public event Action OnGameOver;
    public event Action OnWin;

    bool gameIsOver = false;
    public bool TemporaryPaused { get; set; } = false;

    public bool IsGameOver => gameIsOver;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        nest.Egg.TheEggDied += TheEgg_TheEggDied;
        nest.Baby.BabyDied += Baby_BabyDied;
        player.PlayerDied += Player_PlayerDied;
        dayNight.MidDay += DayNight_MidDay;
    }

    private void DayNight_MidDay(int day)
    {
        if(day == 10)
        {
            Win();
        }
    }

    void Win()
    {
        foreach (var enemy in Enemies)
        {
            if (enemy != null)
                enemy.Die();
        }

        // DO win Animation

        OnWin?.Invoke();
    }

    void GameOver()
    {
        if (gameIsOver)
            return;
        gameIsOver = true;

        Debug.Log("GameOver");
        Player.Die();
     

        OnGameOver?.Invoke();
    }

    private void Player_PlayerDied()
    {
        Debug.Log("Player died");
        GameOver();
    }

    private void Baby_BabyDied()
    {
        Debug.Log("Baby died");
        GameOver();
    }

    private void TheEgg_TheEggDied()
    {
        Debug.Log("Egg died");
        GameOver();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExtiGame()
    {
        Application.Quit();
    }
}
