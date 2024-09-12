using UnityEngine;

public class PowerCoin : Coin
{
    public float duration = 8f;

    protected override void Eat()
    {
        Manager.Instance.PowerCoinEaten(this);

        // Воспроизводим звук для PowerCoin через Manager
        Manager.Instance.PlayPowerCoinSound();

        base.Eat(); // Вызываем базовый метод для завершения поедания
    }
}
