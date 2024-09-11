using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSpriteWithDelay : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[0]; // Массив спрайтов
    public float animationTime = 0.25f; // Время показа одного спрайта
    public float loopDelay = 1.0f; // Задержка между циклами
    public bool loop = true; // Включение циклического воспроизведения

    private SpriteRenderer spriteRenderer;
    private int animationFrame;
    private bool isPlaying = true; // Флаг для отслеживания проигрывания анимации

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
        Restart(); // Запускаем анимацию
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
        CancelInvoke(nameof(Advance)); // Останавливаем анимацию при выключении
    }

    private void Start()
    {
        // Начинаем анимацию с задержкой между циклами
        InvokeRepeating(nameof(Advance), animationTime, animationTime);
    }

    private void Advance()
    {
        if (!spriteRenderer.enabled || !isPlaying)
        {
            return;
        }

        animationFrame++;

        if (animationFrame >= sprites.Length)
        {
            if (loop)
            {
                // Если анимация закончилась, ставим на паузу и добавляем задержку перед следующим циклом
                isPlaying = false;
                Invoke(nameof(StartNextLoop), loopDelay);
            }
            animationFrame = 0;
        }

        if (animationFrame >= 0 && animationFrame < sprites.Length)
        {
            spriteRenderer.sprite = sprites[animationFrame];
        }
    }

    private void StartNextLoop()
    {
        isPlaying = true; // Снимаем паузу, и анимация снова проигрывается
        animationFrame = 0;
    }

    public void Restart()
    {
        animationFrame = -1; // Начинаем с первого спрайта
        isPlaying = true; // Убеждаемся, что анимация идет
        Advance(); // Принудительный вызов первого кадра
    }
}
