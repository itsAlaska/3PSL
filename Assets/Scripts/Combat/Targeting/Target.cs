using System;
using UnityEngine;

namespace Combat.Targeting
{
    public class Target : MonoBehaviour
    {
        public event Action<Target> DestroyedEvent;

        private void OnDestroy()
        {
            DestroyedEvent?.Invoke(this);
        }
    }
}
