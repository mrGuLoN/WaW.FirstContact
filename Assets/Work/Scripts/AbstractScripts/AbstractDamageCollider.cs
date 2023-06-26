using UnityEngine;

public class AbstractDamageCollider : MonoBehaviour
{
    public AbstractHealth abstractHealth;

    public void TakeDamage(float damage, Vector3 point, Vector3 direction)
    {
        abstractHealth.TakeDamage( damage,  point,  direction);
    }
}
