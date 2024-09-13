using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    public int points = 10;

    protected virtual void Eat()
    {
        Manager.Instance.CoinEaten(this);

        Manager.Instance.PlayCoinSound();

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("MainChar"))
        {
            Eat();
        }
    }
}
