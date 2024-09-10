using UnityEngine;

[RequireComponent(typeof(Movement))]
public class MainChar : MonoBehaviour
{
    [SerializeField]
    private AnimatedSprite deathSequence;

    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private Movement movement;
    private Vector3 originalScale;
    private bool isFacingLeft = false;

    private GameObject deathPrefab;
    private GameObject mainSpritePrefab;
    private GameObject superSpritePrefab;  // Новый подпрефаб для режима суперсилы

    private void Awake()
    {
        deathPrefab = transform.Find("Death").gameObject;
        mainSpritePrefab = transform.Find("MainSprite").gameObject;
        superSpritePrefab = transform.Find("SuperSprite").gameObject;  // Инициализация SuperSprite

        circleCollider = GetComponent<CircleCollider2D>();
        movement = GetComponent<Movement>();

        originalScale = transform.localScale; // Инициализация оригинального масштаба

        // При старте игры включаем основной спрайт и отключаем все остальные
        deathPrefab.SetActive(false);
        mainSpritePrefab.SetActive(true);
        superSpritePrefab.SetActive(false);  // Отключаем спрайт суперсилы
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            this.movement.SetNextDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            this.movement.SetNextDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            this.movement.SetNextDirection(Vector2.left);
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            this.movement.SetNextDirection(Vector2.right);
            transform.localScale = originalScale;
        }

        RotateCharacter(); // Функция для поворота персонажа в зависимости от направления
    }

    private void RotateCharacter()
    {
        if (movement.direction == Vector2.up)
        {
            this.transform.rotation = isFacingLeft ? Quaternion.Euler(0, 0, -90) : Quaternion.Euler(0, 0, 90);
        }
        else if (movement.direction == Vector2.down)
        {
            this.transform.rotation = isFacingLeft ? Quaternion.Euler(0, 0, 90) : Quaternion.Euler(0, 0, -90);
        }
        else if (movement.direction == Vector2.left)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            isFacingLeft = true;
        }
        else if (movement.direction == Vector2.right)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            isFacingLeft = false;
        }
    }

    public void ResetState()
    {
        enabled = true;

        // Включаем основной спрайт
        mainSpritePrefab.SetActive(true);
        superSpritePrefab.SetActive(false);  // Отключаем спрайт суперсилы

        circleCollider.enabled = true;

        // Отключаем спрайт смерти
        deathPrefab.SetActive(false);

        movement.ResetState();
        gameObject.SetActive(true);
    }

    public void DeathSequence()
    {
        enabled = false;

        // Отключаем все спрайты, кроме спрайта смерти
        mainSpritePrefab.SetActive(false);
        superSpritePrefab.SetActive(false);
        circleCollider.enabled = false;
        movement.enabled = false;

        // Включаем спрайт смерти
        deathPrefab.SetActive(true);
        deathSequence.Restart();  // Если есть логика анимации для спрайта смерти
    }

    // Метод для включения спрайта суперсилы
    public void CollectSuperPoint()
    {
        mainSpritePrefab.SetActive(false);  // Отключаем обычный спрайт
        superSpritePrefab.SetActive(true);  // Включаем суперспрайт
        movement.speedMultiplier = 1.5f;  // Увеличиваем скорость
    }

    // Метод для возврата к обычному спрайту
    public void ResetToNormalSprite()
    {
        superSpritePrefab.SetActive(false);  // Отключаем суперспрайт
        mainSpritePrefab.SetActive(true);    // Включаем обычный спрайт
        movement.speedMultiplier = 1.0f;     // Возвращаем скорость к нормальной
    }
}
