using UnityEngine;
using UnityEngine.Serialization;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    
    private float _verticalVelocity;

    public Vector3 Movement => Vector3.up * _verticalVelocity;

    private void Update()
    {
        if (_verticalVelocity < 0 && controller.isGrounded)
        {
            _verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            _verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
    }
}
