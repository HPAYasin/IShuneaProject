using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Teleport : MonoBehaviour
{
    public Transform connection;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Телепортируем любой объект, попавший в телепорт
        Vector3 position = connection.position;
        position.z = other.transform.position.z;
        other.transform.position = position;

        // Если объект — главный персонаж, проигрываем звук
        if (other.CompareTag("MainChar"))  // Проверка по тегу "MainChar"
        {
            // Воспроизведение звука телепортации через Manager
            Manager.Instance.PlayTeleportSound();
        }
    }
}
