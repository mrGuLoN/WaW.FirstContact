using UnityEngine;

public class AbstractGunScripts : MonoBehaviour
{
    [SerializeField] protected ParticleSystem _splash;
    [SerializeField] protected AnimatorOverrideController _gunController;
    public virtual void Fire()
    {
        
    }
}
