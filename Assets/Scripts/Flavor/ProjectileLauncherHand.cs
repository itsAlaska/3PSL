using UnityEngine;

namespace Flavor
{
    public class ProjectileLauncherHand : MonoBehaviour
    {
        [SerializeField] private ProjectileLauncher projectileLauncher;

        private void LaunchProjectile()
        {
            projectileLauncher.LaunchProjectile();
        }
    }
}