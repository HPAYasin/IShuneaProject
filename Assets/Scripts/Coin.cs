using UnityEngine;

public class Coin : MonoBehaviour
{
    public int points = 10;

    protected virtual void Eat()
    {
        FindObjectOfType<Manager>().CoinEaten(this);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("MainChar"))
        {
            Eat();
        }
    }
}
