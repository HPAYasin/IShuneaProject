using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSpriteWithDelay : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[0]; 
    public float animationTime = 0.25f; 
    public float loopDelay = 1.0f; 
    public bool loop = true; 

    private SpriteRenderer spriteRenderer;
    private int animationFrame;
    private bool isPlaying = true; 

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
        Restart(); 
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
        CancelInvoke(nameof(Advance)); 
    }

    private void Start()
    {
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
        isPlaying = true; 
        animationFrame = 0;
    }

    public void Restart()
    {
        animationFrame = -1; 
        isPlaying = true; 
        Advance(); 
    }
}
