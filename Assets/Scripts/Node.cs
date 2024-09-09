using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public LayerMask colLayer;
    public List<Vector2> availableDirection {  get; private set; }
    
    private void Start()
    {
        this.availableDirection = new List<Vector2>();

        CheckAD(Vector2.up);
        CheckAD(Vector2.down);
        CheckAD(Vector2.left);
        CheckAD(Vector2.right);
    }

    private void CheckAD(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.5f, 0.0f, direction, 1.5f, this.colLayer);
        if (hit.collider == null)
        {
            this.availableDirection.Add(direction);
        }
    }
}
