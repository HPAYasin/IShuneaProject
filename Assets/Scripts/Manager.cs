using UnityEngine;
using UnityEngine.UI;

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

        enemyMultiplier++;
    }

    public void CoinEaten(Coin coin)
    {
        coin.gameObject.SetActive(false);

        SetScore(score + coin.points);

        if (!HasRemainingCoins())
        {
            mainchar.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3f);
        }
    }

    public void PowerCoinEaten(PowerCoin coin)
    {
        // Включаем frightened режим для всех призраков
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].frightened.Enable(coin.duration);
        }

        // Убираем съеденный PowerCoin
        CoinEaten(coin);

        // Меняем спрайт MainChar на суперспрайт
        mainchar.CollectSuperPoint();

        // Если таймер уже был запущен, сбрасываем его, чтобы продлить действие суперсилы
        CancelInvoke(nameof(ResetMainCharSprite));

        // Запускаем новый таймер для сброса спрайта и возврата к обычному состоянию
        Invoke(nameof(ResetMainCharSprite), coin.duration);
    }

    private void ResetMainCharSprite()
    {
        mainchar.ResetToNormalSprite();
        ResetEnemyMultiplier();  // Сбрасываем множитель очков для призраков
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

}
