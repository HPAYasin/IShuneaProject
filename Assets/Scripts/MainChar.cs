using UnityEngine;

[RequireComponent(typeof(Movement))]
public class MainChar : MonoBehaviour
{
    public Movement movement { get; private set; }
    private Vector3 originalScale;
    private bool isFacingLeft = false;

    private void Awake()
    {
        this.movement = GetComponent<Movement>();
        originalScale = transform.localScale;
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

        RotateCharacter(); // функция для поворота персонажа в зависимости от направления
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
        this.movement.ResetState();
        this.gameObject.SetActive(true);
    }
}
