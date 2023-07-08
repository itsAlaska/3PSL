using UnityEngine;

namespace Player
{
    public class FootstepSounds : MonoBehaviour
    {
        [SerializeField]private AudioSource footstepSound;

        public void PlayFootstepSound()
        {
            footstepSound.Play();
        }
    }
}
