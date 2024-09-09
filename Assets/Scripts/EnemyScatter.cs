using UnityEngine;

public class EnemyScatter : EnemyBeh
{
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled && !enemy.fear.enabled)
        {
            int index = Random.Range(0, node.availableDirection.Count);

            if (node.availableDirection.Count > 1 && node.availableDirection[index] == -enemy.movement.direction)
            {
                index++;

                if (index >= node.availableDirection.Count)
                {
                    index = 0;
                }
            }

            enemy.movement.SetDirection(node.availableDirection[index]);
        }
    }

}