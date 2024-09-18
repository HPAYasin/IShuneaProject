using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;    
    public Vector3 offset;      
    public bool isFollowing = false;  

    [SerializeField] private float defaultFOV = 60f; 
    private Camera cameraComponent;   

    private void Awake()
    {
        cameraComponent = GetComponent<Camera>();

        SetFOV(defaultFOV);
    }

    private void LateUpdate()
    {
        if (isFollowing && player != null)
        {
            transform.position = player.position + offset;
        }
    }

    public void ToggleFollow(bool follow)
    {
        isFollowing = follow;
    }

    public void SetFOV(float fov)
    {
        if (cameraComponent != null)
        {
            cameraComponent.fieldOfView = fov;
        }
    }

    public float GetFOV()
    {
        return cameraComponent != null ? cameraComponent.fieldOfView : 60f;
    }
}
