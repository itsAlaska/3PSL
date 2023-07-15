using UnityEngine;

namespace Flavor
{
    public class ProjectileLauncher : MonoBehaviour
    {
        [SerializeField] private GameObject projectilePrefab;
        // [SerializeField] private float launchSpeed = 10f;
        private Transform _target;

        public void LaunchProjectile()
        {
            // var newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // var direction = _target != null
            //     ? (_target.position - transform.position).normalized
            //     :
            //     // Use the forward direction of the launcher object
            //     transform.forward;

            // var projectileRigidbody = newProjectile.GetComponent<Rigidbody>();
            // projectileRigidbody.velocity = direction * launchSpeed;
        }
    }
}