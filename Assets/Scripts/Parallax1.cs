using UnityEngine;

public class Parallax1 : MonoBehaviour
{
    public Transform[] clouds;        // Массив объектов с облаками
    public float[] moveSpeeds;        // Скорость движения для каждого облака
    public float moveDistance = 2f;   // Максимальная дистанция движения в одну сторону
    public float smoothing = 1f;      // Скорость сглаживания движения

    private Vector3[] startingPositions; // Исходные позиции облаков
    private bool[] movingRight;          // Флаги, чтобы отслеживать направление движения

    private void Start()
    {
        startingPositions = new Vector3[clouds.Length];
        movingRight = new bool[clouds.Length];

        // Запоминаем начальные позиции облаков и устанавливаем начальные направления
        for (int i = 0; i < clouds.Length; i++)
        {
            startingPositions[i] = clouds[i].position;
            movingRight[i] = true; // Начинаем движение вправо
        }
    }

    private void Update()
    {
        // Обновляем положение каждого облака
        for (int i = 0; i < clouds.Length; i++)
        {
            // Двигаем облако в зависимости от направления
            float moveStep = moveSpeeds[i] * Time.deltaTime;

            if (movingRight[i])
            {
                clouds[i].position += new Vector3(moveStep, 0, 0);

                // Если облако достигло максимального смещения вправо, меняем направление
                if (clouds[i].position.x >= startingPositions[i].x + moveDistance)
                {
                    movingRight[i] = false; // Движение влево
                }
            }
            else
            {
                clouds[i].position -= new Vector3(moveStep, 0, 0);

                // Если облако достигло максимального смещения влево, меняем направление
                if (clouds[i].position.x <= startingPositions[i].x - moveDistance)
                {
                    movingRight[i] = true; // Движение вправо
                }
            }

            // Плавность движения
            clouds[i].position = Vector3.Lerp(clouds[i].position, clouds[i].position, smoothing * Time.deltaTime);
        }
    }
}
