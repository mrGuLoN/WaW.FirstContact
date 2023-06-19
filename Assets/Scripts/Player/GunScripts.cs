using UnityEngine;

public class GunScripts : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform gunTransform;
    [SerializeField] private float damage;
    [SerializeField] private GameObject bullet;

    public void Fire()
    {
        animator.SetTrigger("Fire");
        GameObject go = Instantiate(bullet, gunTransform.position, gunTransform.rotation);
        go.GetComponent<Rigidbody>().velocity = gunTransform.forward * 30f;
    }
}