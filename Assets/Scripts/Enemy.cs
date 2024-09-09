using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Movement movement {  get; private set; }
    public EnemyBasement basement { get; private set; }
    public EnemyChasing chasing { get; private set; }
    public EnemyScatter scatter { get; private set; }
    public Enemyfear fear { get; private set; }

    public EnemyBeh initialBeh;
    public Transform target;

    public int points = 200;

    private void Awake()
    {
        this.movement = GetComponent<Movement>();
        this.basement = GetComponent<EnemyBasement>();
        this.chasing = GetComponent<EnemyChasing>();
        this.scatter = GetComponent<EnemyScatter>();
        this.fear = GetComponent<Enemyfear>();
    }
    public void Start()
    {
        ResetState();
    }
    public void ResetState()
    {
        this.gameObject.SetActive(true);
        this.movement.ResetState();

        this.fear.Disable();
        this.chasing.Disable();
        this.scatter.Enable();
        
        if (this.basement != this.initialBeh)
        {
            this.basement.Disable();

        }
        if (this.initialBeh != null)
        {
            this.initialBeh.Enable();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("MainChar"))
        {
            if (this.fear.enabled)
            {
                FindObjectOfType<Manager>().EnemyEaten(this);
            } else {
                FindObjectOfType<Manager>().MainCharEaten();
            }
        }
    }
}
