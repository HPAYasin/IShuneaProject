using UnityEngine;

[RequireComponent(typeof(Enemy))]

public abstract class EnemyBeh : MonoBehaviour
{
    public Enemy enemy {  get; private set; }
    public float duration;    

    private void Awake()
    {
        this.enemy = GetComponent<Enemy>();
        this.enabled = false;
    }

    public void Enable()
    {
        Enable(this.duration);
    }

    public virtual void Enable(float duration)
    {
        this.enabled = true;

        CancelInvoke();

        Invoke(nameof(Disable), duration);
    }

    public virtual void Disable()
    {
        this.enabled = false;

        CancelInvoke();

    }
}
