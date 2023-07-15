using UnityEngine;

namespace Flavor.Visual
{
    public class Spinner : MonoBehaviour
    {
        public float minRotationSpeed = 20f; // Minimum rotation speed
        public float maxRotationSpeed = 60f; // Maximum rotation speed
        public float minChangeInterval = 2f; // Minimum interval to change rotation direction
        public float maxChangeInterval = 5f; // Maximum interval to change rotation direction
        public float rotationTransitionSpeed = 10f; // Speed of rotation transition
        public float rotationDamping = 2f; // Damping factor for rotation transition

        private Vector3 rotationAxis; // Current rotation axis
        private float rotationSpeed; // Current rotation speed
        private float changeInterval; // Randomized interval to change rotation direction
        private float currentChangeTime; // Current time for changing rotation direction

        private Quaternion targetRotation; // Target rotation for smooth transition
        private Quaternion currentRotationVelocity; // Current rotation velocity for damping

        private void Start()
        {
            // Randomize the initial rotation axis, speed, and change interval
            rotationAxis = Random.insideUnitSphere.normalized;
            rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
            changeInterval = Random.Range(minChangeInterval, maxChangeInterval);
            currentChangeTime = changeInterval;
        }

        private void Update()
        {
            // Update the current change time
            currentChangeTime -= Time.deltaTime;

            if (currentChangeTime <= 0f)
            {
                // Randomize the rotation axis, speed, and change interval
                rotationAxis = Random.insideUnitSphere.normalized;
                rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
                changeInterval = Random.Range(minChangeInterval, maxChangeInterval);

                // Reset the change time
                currentChangeTime = changeInterval;

                // Calculate the target rotation based on the new rotation axis
                targetRotation = Quaternion.FromToRotation(transform.up, rotationAxis) * transform.rotation;
            }

            // Calculate the rotation amount based on the rotation speed
            var rotationAmount = rotationSpeed * Time.deltaTime;

            // Smoothly rotate towards the target rotation with damping
            transform.rotation = SmoothDamp(transform.rotation, targetRotation, ref currentRotationVelocity,
                rotationTransitionSpeed, rotationDamping);

            // Rotate the object around the current rotation axis
            transform.Rotate(rotationAxis, rotationAmount, Space.World);
        }

        private Quaternion SmoothDamp(Quaternion current, Quaternion target, ref Quaternion currentVelocity,
            float smoothTime, float damping)
        {
            var maxRadiansDelta = Mathf.Deg2Rad * Mathf.Abs(Quaternion.Angle(current, target));
            var tau = 2f / smoothTime;

            var dampening = Mathf.Max(1f - damping * tau, 0.0001f);
            var omega = tau * Mathf.Sqrt(1f - dampening * dampening);

            var result =
                Quaternion.RotateTowards(current, target, maxRadiansDelta * Mathf.Rad2Deg * Time.deltaTime * omega);

            return result;
        }
    }
}