using UnityEngine;

public class PowerCoin : Coin
{
    public float duration = 8f;

    protected override void Eat()
    {
        Manager.Instance.PowerCoinEaten(this);

        // ������������� ���� ��� PowerCoin ����� Manager
        Manager.Instance.PlayPowerCoinSound();

        base.Eat(); // �������� ������� ����� ��� ���������� ��������
    }
}
