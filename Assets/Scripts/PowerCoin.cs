using UnityEngine;

public class PowerCoin : Coin
{
    public float duration = 8f;

    protected override void Eat()
    {
        Manager.Instance.PowerCoinEaten(this);
        base.Eat(); // Убедитесь, что вызывается базовый метод для отключения PowerCoin
    }
}
