using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace Combat.Targeting
{
    public class Targeter : MonoBehaviour
    {
        public List<Target> targets = new();

        [SerializeField] private Material inRangeShader;
        [SerializeField] private Material targetedShader;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Target>(out var target))
            {
                targets.Add(target);
                other.gameObject.layer = 6;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Target>(out var target))
            {
                targets.Remove(target);
                other.gameObject.layer = 0;
            }
        }
    }
}