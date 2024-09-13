using UnityEngine;

public class Parallax1 : MonoBehaviour
{
    public Transform[] clouds;        
    public float[] moveSpeeds;        
    public float moveDistance = 2f;   
    public float smoothing = 1f;      

    private Vector3[] startingPositions; 
    private bool[] movingRight;          

    private void Start()
    {
        startingPositions = new Vector3[clouds.Length];
        movingRight = new bool[clouds.Length];

        for (int i = 0; i < clouds.Length; i++)
        {
            startingPositions[i] = clouds[i].position;
            movingRight[i] = true; // Начинаем движение вправо
        }
    }

    private void Update()
    {
        for (int i = 0; i < clouds.Length; i++)
        {
            float moveStep = moveSpeeds[i] * Time.deltaTime;

            if (movingRight[i])
            {
                clouds[i].position += new Vector3(moveStep, 0, 0);

                if (clouds[i].position.x >= startingPositions[i].x + moveDistance)
                {
                    movingRight[i] = false; 
                }
            }
            else
            {
                clouds[i].position -= new Vector3(moveStep, 0, 0);

                if (clouds[i].position.x <= startingPositions[i].x - moveDistance)
                {
                    movingRight[i] = true; 
                }
            }

            clouds[i].position = Vector3.Lerp(clouds[i].position, clouds[i].position, smoothing * Time.deltaTime);
        }
    }
}
