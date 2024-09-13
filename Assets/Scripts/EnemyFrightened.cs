using UnityEngine;

public class EnemyFrightened : EnemyBehavior
{
    public SpriteRenderer body;
    public SpriteRenderer blue;
    public SpriteRenderer white;

    private bool eaten;

    public override void Enable(float duration)
    {
        base.Enable(duration);

        body.enabled = false;
        blue.enabled = true;
        white.enabled = false;

        Invoke(nameof(Flash), duration / 2f);
    }

    public override void Disable()
    {
        base.Disable();

        body.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    private void Eaten()
    {
        eaten = true;
        enemy.SetPosition(enemy.home.inside.position);
        enemy.home.Enable(duration);

        body.enabled = false;
        blue.enabled = false;
        white.enabled = false;
    }

    private void Flash()
    {
        if (!eaten)
        {
            blue.enabled = false;
            white.enabled = true;
            white.GetComponent<AnimatedSprite>().Restart();
        }
    }

    private void OnEnable()
    {
        blue.GetComponent<AnimatedSprite>().Restart();
        enemy.movement.speedMultiplier = 0.5f;
        eaten = false;
    }

    private void OnDisable()
    {
        enemy.movement.speedMultiplier = 1f;
        eaten = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            foreach (Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (enemy.target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }

            enemy.movement.SetDirection(direction);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("MainChar"))
        {
            if (enabled)
            {
                Eaten();
            }
        }
    }

}
