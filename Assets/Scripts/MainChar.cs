using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Movement))]
public class MainChar : MonoBehaviour
{
    [SerializeField]
    private AnimatedSprite deathSequence;

    [SerializeField]
    private AudioSource footstepAudioSource1;  
    [SerializeField]
    private AudioSource footstepAudioSource2;  
    [SerializeField]
    private AudioClip footstepClip1;  
    [SerializeField]
    private AudioClip footstepClip2;  
    [SerializeField]
    private float footstepDelay = 0.5f;  

    [SerializeField]
    private AudioSource swordAudioSource;  
    [SerializeField]
    private AudioClip swordSwingSound1;  
    [SerializeField]
    private AudioClip swordSwingSound2; 
    [SerializeField]
    private AudioClip swordUnsheatheSound; 
    [SerializeField]
    private float swordSwingDelay = 0.5f;  

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
    private Coroutine swordSwingCoroutine;  
    private int currentSwordSwingIndex = 0;  

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

        StopSwordSwingSound();  
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

    public void CollectSuperPoint()
    {
        mainSpritePrefab.SetActive(false);
        superSpritePrefab.SetActive(true);
        movement.speedMultiplier = 1.5f;  

        if (swordUnsheatheSound != null)
        {
            swordAudioSource.PlayOneShot(swordUnsheatheSound);
        }

        if (swordSwingCoroutine == null)
        {
            swordSwingCoroutine = StartCoroutine(PlaySwordSwingSounds());
        }
    }

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

            currentSwordSwingIndex = (currentSwordSwingIndex + 1) % 2;

            yield return new WaitForSeconds(swordSwingDelay);
        }
    }

    public void ResetToNormalSprite()
    {
        superSpritePrefab.SetActive(false);
        mainSpritePrefab.SetActive(true);
        movement.speedMultiplier = 1.0f;

        StopSwordSwingSound(); 
    }

    private void StopSwordSwingSound()
    {
        if (swordSwingCoroutine != null)
        {
            StopCoroutine(swordSwingCoroutine);  
            swordSwingCoroutine = null;  
        }
    }
}
