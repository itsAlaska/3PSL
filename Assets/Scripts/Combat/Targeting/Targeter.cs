using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Combat.Targeting
{
    public class Targeter : MonoBehaviour
    {
        [SerializeField] private CinemachineTargetGroup cineTargetGroup;
        
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
            target.DestroyedEvent += RemoveTarget;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Target>(out var target)) return;
            
            _targets.Remove(target);
            other.gameObject.layer = 0;
            RemoveTarget(target);
        }

        public bool SelectTarget()
        {
            if (_targets.Count == 0) return false;

            CurrentTarget = _targets[0];
            cineTargetGroup.AddMember(CurrentTarget.transform, 1, 2);
            
            return true;
        }

        public void Cancel()
        {
            if (CurrentTarget == null) return;
            
            cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        private void RemoveTarget(Target target)
        {
            if (CurrentTarget == target)
            {
                cineTargetGroup.RemoveMember(CurrentTarget.transform);
                CurrentTarget = null;
            }

            target.DestroyedEvent -= RemoveTarget;
        }
    }
}