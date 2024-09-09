using UnityEngine;

public class PowerCoin : Coin
{
    public float duration = 8.0f;
    protected override void Eat()
    {
        FindObjectOfType<Manager>().PowerCoinEaten(this);
    }
}
