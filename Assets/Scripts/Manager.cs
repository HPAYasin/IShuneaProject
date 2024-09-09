using UnityEngine;
using UnityEngine.UI;


public class Manager : MonoBehaviour
{
    public Enemy[] enemy;
    public MainChar mainchar;
    public Transform coins;

    public int score { get; private set; }
    
    public int lives { get; private set; }

    public int enemyMultiplier { get; private set; } = 1;

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (this.lives <= 0 && Input.anyKeyDown) {
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
        foreach (Transform coin in this.coins)
        {
            coin.gameObject.SetActive(true);
        }

        ResetState();
        
    }

    private void ResetState()
    {
        ResetEnemyMultiplier();

        for (int i = 0; i < this.enemy.Length; i++)
        {
            this.enemy[i].ResetState();
        }
        this.mainchar.ResetState();
    }
    
    private void GameOver()
    {
        for (int i = 0; i < this.enemy.Length; i++)
        {
            this.enemy[i].gameObject.SetActive(false);
        }
        this.mainchar.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    { 
        this.score = score;
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
    }

    public void EnemyEaten(Enemy enemy)
    {
        int points = enemy.points * this.enemyMultiplier;
        SetScore(this.score + points);
        this.enemyMultiplier++;
    }

    public void MainCharEaten()
    {
        this.mainchar.gameObject.SetActive(false);
        SetLives(this.lives - 1);
        if (this.lives > 0)
        {
            Invoke(nameof(ResetState), 3.0f);
        }
        else
        {
            GameOver();
        }
    }
    
    public void CoinEaten(Coin coin)
    {
        coin.gameObject.SetActive(false);
        SetScore(this.score + coin.points);
        if (!AllCoins())
        {
            this.mainchar.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3.0f);
        }
    }

    public void PowerCoinEaten(PowerCoin coin)
    {
        CoinEaten(coin);
        CancelInvoke();
        Invoke(nameof(ResetEnemyMultiplier), coin.duration);

    }

    private bool AllCoins()
    {
        foreach (Transform coin in this.coins)
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
        this.enemyMultiplier = 1;

    }

}
