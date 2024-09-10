using UnityEngine;

[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(Movement))]
public class Enemy : MonoBehaviour
{
    public Movement movement { get; private set; }
    public EnemyHome home { get; private set; }
    public EnemyScatter scatter { get; private set; }
    public EnemyChase chase { get; private set; }
    public EnemyFrightened frightened { get; private set; }
    public EnemyBehavior initialBehavior;
    public Transform target;
    public int points = 200;

    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;
    private bool isFacingLeft = false;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        home = GetComponent<EnemyHome>();
        scatter = GetComponent<EnemyScatter>();
        chase = GetComponent<EnemyChase>();
        frightened = GetComponent<EnemyFrightened>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalScale = transform.localScale; // Сохраняем оригинальный масштаб
    }

    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        FlipAndRotate();
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        movement.ResetState();

        frightened.Disable();
        chase.Disable();
        scatter.Enable();

        if (home != initialBehavior)
        {
            home.Disable();
        }

        if (initialBehavior != null)
        {
            initialBehavior.Enable();
        }

        // Сброс зеркалирования при сбросе состояния
        transform.localScale = originalScale;
        transform.rotation = Quaternion.identity;  // Сбрасываем поворот
    }

    private void FlipAndRotate()
    {
        // Логика зеркалирования влево и вправо
        if (movement.direction == Vector2.left)
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z); // Зеркалируем по оси X
            isFacingLeft = true;
        }
        else if (movement.direction == Vector2.right)
        {
            transform.localScale = originalScale; // Возвращаем оригинальный масштаб
            isFacingLeft = false;
        }

        // Логика поворота вверх и вниз с учётом зеркалирования
        if (movement.direction == Vector2.up)
        {
            // Поворачиваем персонажа в зависимости от того, смотрит ли он влево
            if (isFacingLeft)
            {
                transform.rotation = Quaternion.Euler(0, 0, -90); // Поворачиваем против часовой стрелки
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);  // Поворачиваем по часовой стрелке
            }
        }
        else if (movement.direction == Vector2.down)
        {
            if (isFacingLeft)
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);  // Поворачиваем по часовой стрелке
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, -90); // Поворачиваем против часовой стрелки
            }
        }
        else if (movement.direction == Vector2.left || movement.direction == Vector2.right)
        {
            // Для движения влево и вправо сбрасываем поворот
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }


    public void SetPosition(Vector3 position)
    {
        // Keep the z-position the same since it determines draw depth
        position.z = transform.position.z;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("MainChar"))
        {
            if (frightened.enabled)
            {
                Manager.Instance.EnemyEaten(this);
            }
            else
            {
                Manager.Instance.MainCharEaten();
            }
        }
    }
}
