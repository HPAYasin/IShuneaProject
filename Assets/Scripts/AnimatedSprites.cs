using UnityEngine;

public class AnimatedSprites : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    public Sprite[] sprites;
    public float animationTime = 0.25f;
    public int animationFrame { get; private set; }
    public bool loop = true;

    private void Awake()
    {
        // Поиск компонента SpriteRenderer в дочерних объектах
        this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Проверка, был ли найден SpriteRenderer
        if (this.spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer не найден на дочернем объекте!");
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(Advance), this.animationTime, this.animationTime);
    }

    private void Advance()
    {
        if (this.spriteRenderer == null || !this.spriteRenderer.enabled)
        {
            return;
        }

        this.animationFrame++;

        if (this.animationFrame >= this.sprites.Length && this.loop)
        {
            this.animationFrame = 0;
        }

        if (this.animationFrame >= 0 && this.animationFrame < this.sprites.Length)
        {
            this.spriteRenderer.sprite = this.sprites[this.animationFrame];
        }
    }

    public void Restart()
    {
        this.animationFrame = -1;
        Advance();
    }
}
