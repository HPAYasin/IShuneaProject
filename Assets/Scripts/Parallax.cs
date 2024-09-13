using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform[] clouds;        
    public float[] moveSpeeds;        
    public float moveDistance = 2f;   
    public float smoothing = 1f;      

    private Vector3[] startingPositions; 
    private bool[] movingUp;             

    private void Start()
    {
        startingPositions = new Vector3[clouds.Length];
        movingUp = new bool[clouds.Length];

        for (int i = 0; i < clouds.Length; i++)
        {
            startingPositions[i] = clouds[i].position;
            movingUp[i] = true; 
        }
    }

    private void Update()
    {
        for (int i = 0; i < clouds.Length; i++)
        {
            float moveStep = moveSpeeds[i] * Time.deltaTime;

            if (movingUp[i])
            {
                clouds[i].position += new Vector3(0, moveStep, 0); 

                if (clouds[i].position.y >= startingPositions[i].y + moveDistance)
                {
                    movingUp[i] = false; // Движение вниз
                }
            }
            else
            {
                clouds[i].position -= new Vector3(0, moveStep, 0); 

                if (clouds[i].position.y <= startingPositions[i].y - moveDistance)
                {
                    movingUp[i] = true; 
                }
            }

            clouds[i].position = Vector3.Lerp(clouds[i].position, clouds[i].position, smoothing * Time.deltaTime);
        }
    }
}
