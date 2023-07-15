using Combat.Targeting;
using StateMachines.Player;
using UnityEngine;

namespace Flavor
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;

        private PlayerStateMachine _stateMachine;
        private Targeter _targeter;
        private Transform _player;
        private float _maxDistance;
        private bool _faceTarget = false;

        private void Start()
        {
            // Find and assign the player object
            _player = GameObject.FindGameObjectWithTag("Player").transform;
            _stateMachine = _player.GetComponent<PlayerStateMachine>();
            _targeter = _stateMachine.Targeter;

            if (_targeter != null)
            {
                // Get the radius of the targeting sphere
                var sphereCollider = _targeter.GetComponent<SphereCollider>();

                if (sphereCollider != null)
                    _maxDistance = sphereCollider.radius;
                else
                    Debug.LogWarning("Targeting sphere collider not found!");
            }
            else
            {
                Debug.LogWarning("Targeting sphere object not assigned!");
            }
        }

        private void Update()
        {
            if (_player != null && _targeter != null)
            {
                // Get the player's forward direction
                var direction = ProjectileDirection();

                if (_faceTarget == true) _player.transform.rotation = Quaternion.LookRotation(direction);

                // Move the projectile forward in the player's forward direction
                transform.Translate(direction * speed * Time.deltaTime, Space.World);

                // Calculate the distance between the projectile and the player
                var distance = Vector3.Distance(transform.position, _player.position);

                // Check if the projectile has reached the maximum distance
                if (distance >= _maxDistance) Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("Player object or targeting sphere object not found!");
            }
        }

        private Vector3 ProjectileDirection()
        {
            if (_stateMachine.Targeter.Targets.Count == 0)
            {
                _faceTarget = false;
                return _player.forward;
            }

            _faceTarget = true;
            var enemyPos =
                _stateMachine.Targeter.Targets[0].transform.position - _player.transform.position;
            enemyPos.y = 0;

            return enemyPos.normalized;
        }
    }
}