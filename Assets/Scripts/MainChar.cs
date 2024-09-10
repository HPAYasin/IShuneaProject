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
    private GameObject superSpritePrefab;  // ����� ��������� ��� ������ ���������

    private void Awake()
    {
        deathPrefab = transform.Find("Death").gameObject;
        mainSpritePrefab = transform.Find("MainSprite").gameObject;
        superSpritePrefab = transform.Find("SuperSprite").gameObject;  // ������������� SuperSprite

        circleCollider = GetComponent<CircleCollider2D>();
        movement = GetComponent<Movement>();

        originalScale = transform.localScale; // ������������� ������������� ��������

        // ��� ������ ���� �������� �������� ������ � ��������� ��� ���������
        deathPrefab.SetActive(false);
        mainSpritePrefab.SetActive(true);
        superSpritePrefab.SetActive(false);  // ��������� ������ ���������
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

        RotateCharacter(); // ������� ��� �������� ��������� � ����������� �� �����������
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

        // �������� �������� ������
        mainSpritePrefab.SetActive(true);
        superSpritePrefab.SetActive(false);  // ��������� ������ ���������

        circleCollider.enabled = true;

        // ��������� ������ ������
        deathPrefab.SetActive(false);

        movement.ResetState();
        gameObject.SetActive(true);
    }

    public void DeathSequence()
    {
        enabled = false;

        // ��������� ��� �������, ����� ������� ������
        mainSpritePrefab.SetActive(false);
        superSpritePrefab.SetActive(false);
        circleCollider.enabled = false;
        movement.enabled = false;

        // �������� ������ ������
        deathPrefab.SetActive(true);
        deathSequence.Restart();  // ���� ���� ������ �������� ��� ������� ������
    }

    // ����� ��� ��������� ������� ���������
    public void CollectSuperPoint()
    {
        mainSpritePrefab.SetActive(false);  // ��������� ������� ������
        superSpritePrefab.SetActive(true);  // �������� �����������
        movement.speedMultiplier = 1.5f;  // ����������� ��������
    }

    // ����� ��� �������� � �������� �������
    public void ResetToNormalSprite()
    {
        superSpritePrefab.SetActive(false);  // ��������� �����������
        mainSpritePrefab.SetActive(true);    // �������� ������� ������
        movement.speedMultiplier = 1.0f;     // ���������� �������� � ����������
    }
}
