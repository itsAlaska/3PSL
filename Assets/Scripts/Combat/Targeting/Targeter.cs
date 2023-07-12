using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat.Targeting
{
    public class Targeter : MonoBehaviour
    {
        public List<Target> targets = new List<Target>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Target>(out var target))
            {
                targets.Add(target);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Target>(out var target))
            {
                targets.Remove(target);
            }
        }
    }
}
