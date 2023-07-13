using UnityEngine;

namespace Flavor
{
    public class Footsteps : MonoBehaviour
    {
        [SerializeField] private AudioSource footstepSound;

        private void PlayFootstepSound()
        {
            footstepSound.Play();
        }
    }
}