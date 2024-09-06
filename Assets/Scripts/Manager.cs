using UnityEngine;

public class Manager : MonoBehaviour
{
    public Enemy[] enemy;
    public MainChar mainchar;
    public Transform cols;

    public int score { get; private set; }
    
    public int lives { get; private set; }

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
        foreach (Transform col in this.cols)
        {
            col.gameObject.SetActive(true);
        }

        ResetState();
        
    }

    private void ResetState()
    {
        for (int i = 0; i < this.enemy.Length; i++)
        {
            this.enemy[i].gameObject.SetActive(true);
        }
        this.mainchar.gameObject.SetActive(true);
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
        SetScore(this.score + enemy.points);
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
}
