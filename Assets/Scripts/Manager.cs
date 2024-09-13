using UnityEngine;
using UnityEngine.SceneManagement;  
using UnityEngine.UI;
using System.Collections;
using System;

[DefaultExecutionOrder(-100)]
public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }

    [SerializeField] private Enemy[] enemies;
    [SerializeField] private MainChar mainchar;
    [SerializeField] private Transform coins;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;

    [Header("Audio")]
    [SerializeField] private AudioSource coinAudioSource;
    [SerializeField] private AudioSource powerCoinAudioSource;
    [SerializeField] private AudioSource teleportAudioSource;
    [SerializeField] private AudioSource playerDeathAudioSource;
    [SerializeField] private AudioSource enemyKillAudioSource;
    [SerializeField] private AudioSource gameOverAudioSource;  
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip powerCoinSound;
    [SerializeField] private AudioClip playerDeathSound;
    [SerializeField] private AudioClip teleportSound;  
    [SerializeField] private AudioClip enemyKillSound;
    [SerializeField] private AudioClip gameOverSound;  
    [SerializeField] private float coinSoundDelay = 0.5f;

    private bool canPlayCoinSound = true;

    public int score { get; private set; } = 0;
    public int lives { get; private set; } = 3;

    private int enemyMultiplier = 1;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        gameOverText.enabled = false;

        foreach (Transform coin in coins)
        {
            coin.gameObject.SetActive(true);
        }

        ResetState();
    }

    private void ResetState()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].ResetState();
        }

        mainchar.ResetState();
    }

    private void GameOver()
    {
        gameOverText.enabled = true;

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].gameObject.SetActive(false);
        }

        mainchar.gameObject.SetActive(false);

        if (gameOverSound != null)
        {
            gameOverAudioSource.PlayOneShot(gameOverSound);
        }

        CheckNewRecord(score);
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0');
    }

    public void MainCharEaten()
    {
        mainchar.DeathSequence();

        if (playerDeathSound != null)
        {
            playerDeathAudioSource.PlayOneShot(playerDeathSound);
        }

        SetLives(lives - 1);

        if (lives > 0)
        {
            Invoke(nameof(ResetState), 3f);
        }
        else
        {
            GameOver();
        }
    }

    public void EnemyEaten(Enemy enemy)
    {
        int points = enemy.points * enemyMultiplier;
        SetScore(score + points);

        if (enemyKillSound != null)
        {
            enemyKillAudioSource.PlayOneShot(enemyKillSound);
        }

        enemyMultiplier++;
    }

    public void CoinEaten(Coin coin)
    {
        coin.gameObject.SetActive(false);

        SetScore(score + coin.points);

        PlayCoinSound();

        if (!HasRemainingCoins())
        {
            mainchar.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3f);
        }
    }

    public void PowerCoinEaten(PowerCoin coin)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].frightened.Enable(coin.duration);
        }

        CoinEaten(coin);
        mainchar.CollectSuperPoint();

        PlayPowerCoinSound();

        CancelInvoke(nameof(ResetMainCharSprite));
        Invoke(nameof(ResetMainCharSprite), coin.duration);
    }

    private void ResetMainCharSprite()
    {
        mainchar.ResetToNormalSprite();
        ResetEnemyMultiplier();
    }

    private bool HasRemainingCoins()
    {
        foreach (Transform coin in coins)
        {
            if (coin.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    private void ResetEnemyMultiplier()
    {
        enemyMultiplier = 1;
    }

    public void PlayCoinSound()
    {
        if (coinSound != null && canPlayCoinSound)
        {
            StartCoroutine(PlayCoinSoundWithDelay());
        }
    }

    private IEnumerator PlayCoinSoundWithDelay()
    {
        coinAudioSource.PlayOneShot(coinSound);
        canPlayCoinSound = false;

        yield return new WaitForSeconds(coinSoundDelay);

        canPlayCoinSound = true;
    }

    public void PlayPowerCoinSound()
    {
        if (powerCoinSound != null && !powerCoinAudioSource.isPlaying)
        {
            powerCoinAudioSource.PlayOneShot(powerCoinSound);
        }
    }

    public void PlayTeleportSound()
    {
        if (teleportSound != null && teleportAudioSource != null)
        {
            teleportAudioSource.PlayOneShot(teleportSound);
        }
    }

    private void CheckNewRecord(int currentScore)
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);

        if (currentScore > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", currentScore);
            PlayerPrefs.SetString("BestScoreDate", DateTime.Now.ToString("yyyy-MM-dd"));
            PlayerPrefs.Save();
            Debug.Log("New record: " + currentScore + " on " + DateTime.Now.ToString("yyyy-MM-dd"));
        }
    }
}
