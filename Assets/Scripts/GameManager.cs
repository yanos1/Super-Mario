using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    public static GameManager game {  get; private set; }
    
    private static int level = 0;
    public static int lives = 3;
    private static int coins = 0;
    private static int  score = 0;
    private static float time = 100f;
    private static string world = "1";

    private Vector3 gravity = new Vector3(0, -1, 0);
    public const float OUT_OF_BOUNDS_POSITION = -8f;
    public TextUpdater textUpdater;
    public GameObject endGameLosePanel;
    public GameObject endGameWinPanel;
    
    private void Awake()
    {
        if (game != null) { return; }
        game = this;
        textUpdater.UpdateTime((int)Math.Ceiling(time));
        textUpdater.UpdateCoin(coins);
        textUpdater.UpdateLives(lives);
        textUpdater.UpdateScore(score);
        textUpdater.UpdateWorld(world);
    }

    private void Update()
    {
        if (time > 0) { time -= Time.deltaTime; }
       textUpdater.UpdateTime((int)Math.Ceiling(time));
    }
    internal int GetLevel()
    {
        return level;
    }

    internal void AddLevel()
    {
        level++;
        GameManager.score += 1000;
        textUpdater.UpdateScore(score);
        SoundManager.sounds.PlayLevelUp();

    }

    public Vector3 AddGravity()
    {
        return gravity;
    }
    internal void LoseHealth()
    {
        lives--;

        textUpdater.UpdateLives(lives);
        SoundManager.sounds.PlayDie();
        if (lives > 0)
        {
            StartCoroutine(ResetRound());
            return;
        }
        
        StartCoroutine(EndGameLose());
        
    }
    public IEnumerator EndGameWin()
    {
        yield return new WaitForSeconds(3f);
        endGameWinPanel.SetActive(true);
         StartCoroutine(reloadGame()); // Wait for reloadGame coroutine to finish
    }  
    
    IEnumerator EndGameLose()
    {
        yield return new WaitForSeconds(2.5f);
        endGameLosePanel.SetActive(true);
        SoundManager.sounds.PlayGameOver();
         StartCoroutine(reloadMap()); // Wait for reloadGame coroutine to finish
    }
    
    IEnumerator reloadMap()
    {
        yield return new WaitForSeconds(7f);
        SceneManager.LoadScene(1);
        endGameLosePanel.SetActive(false);
        TryAgain();
    } 
    public IEnumerator reloadGame()
    {
        yield return new WaitForSeconds(7f);
        ResetStats();
        SceneManager.LoadScene(0);
        endGameLosePanel.SetActive(false);
    }

    internal void SetLevel(int level)
    {
        GameManager.level = level;
    }

    public IEnumerator ResetRound()
    {
        yield return new WaitForSeconds(3);
        time = 100f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

 

public void AddCoins(int coins)
    {
        GameManager.coins += coins;
        textUpdater.UpdateCoin(GameManager.coins);
        SoundManager.sounds.PlayCoin();
    }

    internal void AddScore(int score)
    {
        GameManager.score += score;
        textUpdater.UpdateScore(GameManager.score);
    }

    public void TryAgain()
    {
        lives = 2;
        score = 0;
        time = 100f;
    }

    public float getTimeLeft() { return time; } 

    public void ResetStats()
    {
         level = 0;
         lives = 3;
        coins = 0;
        score = 0;
        time = 100f;
        world = "1";
}

   public void TransformTimeLeftToScore() { }
    //{
    //    while (time >=0)
    //    {
    //        time -= 
    //    }
    //}
}
