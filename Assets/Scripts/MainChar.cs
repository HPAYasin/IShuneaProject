using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Movement))]
public class MainChar : MonoBehaviour
{
    [SerializeField]
    private AnimatedSprite deathSequence;

    [SerializeField]
    private AudioSource footstepAudioSource1;  // Первый AudioSource для звука шагов
    [SerializeField]
    private AudioSource footstepAudioSource2;  // Второй AudioSource для звука шагов
    [SerializeField]
    private AudioClip footstepClip1;  // Первый звуковой клип
    [SerializeField]
    private AudioClip footstepClip2;  // Второй звуковой клип
    [SerializeField]
    private float footstepDelay = 0.5f;  // Задержка между шагами

    [SerializeField]
    private AudioSource swordAudioSource;  // AudioSource для звука меча
    [SerializeField]
    private AudioClip swordSwingSound1;  // Первый звуковой клип для удара мечом
    [SerializeField]
    private AudioClip swordSwingSound2;  // Второй звуковой клип для удара мечом
    [SerializeField]
    private AudioClip swordUnsheatheSound;  // Звуковой клип для вытаскивания меча из ножен
    [SerializeField]
    private float swordSwingDelay = 0.5f;  // Параметр для задержки звука удара мечом

    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private Movement movement;
    private Vector3 originalScale;
    private bool isFacingLeft = false;
    private bool isMoving = false;
    private bool canPlayFootstep = true;
    private int currentFootstepIndex = 0;

    private GameObject deathPrefab;
    private GameObject mainSpritePrefab;
    private GameObject superSpritePrefab;
    private Coroutine swordSwingCoroutine;  // Храним корутину для остановки махов мечом
    private int currentSwordSwingIndex = 0;  // Переменная для чередования звуков ударов мечом

    private void Awake()
    {
        deathPrefab = transform.Find("Death").gameObject;
        mainSpritePrefab = transform.Find("MainSprite").gameObject;
        superSpritePrefab = transform.Find("SuperSprite").gameObject;

        circleCollider = GetComponent<CircleCollider2D>();
        movement = GetComponent<Movement>();

        originalScale = transform.localScale;

        deathPrefab.SetActive(false);
        mainSpritePrefab.SetActive(true);
        superSpritePrefab.SetActive(false);
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

        isMoving = movement.direction != Vector2.zero;

        if (isMoving && canPlayFootstep)
        {
            StartCoroutine(PlayFootstepWithDelay());
        }

        RotateCharacter();
    }

    private IEnumerator PlayFootstepWithDelay()
    {
        if (currentFootstepIndex == 0)
        {
            footstepAudioSource1.clip = footstepClip1;
            footstepAudioSource1.Play();
        }
        else
        {
            footstepAudioSource2.clip = footstepClip2;
            footstepAudioSource2.Play();
        }

        currentFootstepIndex = (currentFootstepIndex + 1) % 2;

        canPlayFootstep = false;

        yield return new WaitForSeconds(footstepDelay);

        canPlayFootstep = true;
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

        mainSpritePrefab.SetActive(true);
        superSpritePrefab.SetActive(false);

        circleCollider.enabled = true;

        deathPrefab.SetActive(false);

        movement.ResetState();
        gameObject.SetActive(true);

        StopSwordSwingSound();  // Останавливаем махи мечом при сбросе состояния
    }

    public void DeathSequence()
    {
        enabled = false;

        mainSpritePrefab.SetActive(false);
        superSpritePrefab.SetActive(false);
        circleCollider.enabled = false;
        movement.enabled = false;

        deathPrefab.SetActive(true);
        deathSequence.Restart();
    }

    // Метод для включения суперсилы
    public void CollectSuperPoint()
    {
        mainSpritePrefab.SetActive(false);
        superSpritePrefab.SetActive(true);
        movement.speedMultiplier = 1.5f;  // Увеличиваем скорость

        // Проигрываем звук вытаскивания меча из ножен один раз
        if (swordUnsheatheSound != null)
        {
            swordAudioSource.PlayOneShot(swordUnsheatheSound);
        }

        // Проверяем, запущена ли корутина махов мечом
        if (swordSwingCoroutine == null)
        {
            // Начинаем проигрывать звуки махов мечом
            swordSwingCoroutine = StartCoroutine(PlaySwordSwingSounds());
        }
    }

    // Корутин для чередования звуков ударов мечом
    private IEnumerator PlaySwordSwingSounds()
    {
        while (true)
        {
            if (currentSwordSwingIndex == 0)
            {
                if (swordSwingSound1 != null)
                {
                    swordAudioSource.PlayOneShot(swordSwingSound1);
                }
            }
            else
            {
                if (swordSwingSound2 != null)
                {
                    swordAudioSource.PlayOneShot(swordSwingSound2);
                }
            }

            // Чередуем звуки ударов мечом
            currentSwordSwingIndex = (currentSwordSwingIndex + 1) % 2;

            // Используем заданную задержку для звука удара мечом
            yield return new WaitForSeconds(swordSwingDelay);
        }
    }

    // Метод для возврата к обычному спрайту
    public void ResetToNormalSprite()
    {
        superSpritePrefab.SetActive(false);
        mainSpritePrefab.SetActive(true);
        movement.speedMultiplier = 1.0f;

        StopSwordSwingSound();  // Останавливаем звуки махов мечом
    }

    // Остановка корутины махов мечом
    private void StopSwordSwingSound()
    {
        if (swordSwingCoroutine != null)
        {
            StopCoroutine(swordSwingCoroutine);  // Останавливаем махи мечом
            swordSwingCoroutine = null;  // Сбрасываем переменную
        }
    }
}
