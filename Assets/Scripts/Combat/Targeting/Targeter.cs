using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Combat.Targeting
{
    public class Targeter : MonoBehaviour
    {
        [SerializeField] private CinemachineTargetGroup cineTargetGroup;

        private Camera _mainCamera;
        private readonly List<Target> _targets = new();
        public Target CurrentTarget { get; private set; }

        public bool isLockedOn;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

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
            
            Target closestTarget = null;
            var closestTargetDistance = Mathf.Infinity;
            
            foreach (var target in _targets)
            {
                Vector2 viewPos = _mainCamera.WorldToViewportPoint(target.transform.position);
                
                if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1) continue;

                var toCenter = viewPos - new Vector2(.5f, .5f);
                if (!(toCenter.sqrMagnitude < closestTargetDistance)) continue;
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }

            if (closestTarget == null) return false;
            
            CurrentTarget = closestTarget;
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