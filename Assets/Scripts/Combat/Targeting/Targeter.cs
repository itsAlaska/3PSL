using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Combat.Targeting
{
    public class Targeter : MonoBehaviour
    {
        private readonly List<Target> _targets = new();
        public Target CurrentTarget { get; private set; }

        public bool isLockedOn;

        private void Reset()
        {
            throw new NotImplementedException();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Target>(out var target)) return;
            _targets.Add(target);
            other.gameObject.layer = 6;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Target>(out var target)) return;
            _targets.Remove(target);
            other.gameObject.layer = 0;
        }

        public bool SelectTarget()
        {
            if (_targets.Count == 0) return false;

            CurrentTarget = _targets[0];
            return true;
        }

        public void Cancel()
        {
            CurrentTarget = null;
        }
    }
}