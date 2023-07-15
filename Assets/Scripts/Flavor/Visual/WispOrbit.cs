using UnityEngine;

namespace Flavor.Visual
{
    public class WispOrbit : MonoBehaviour
    {
        public Vector3 pointA; // Starting point (in local space)
    public Vector3 pointB; // Ending point (in local space)
    public float minXTravelTime = 1f; // Minimum time to travel on the X-axis
    public float maxXTravelTime = 3f; // Maximum time to travel on the X-axis
    public float minYTravelTime = 1f; // Minimum time to travel on the Y-axis
    public float maxYTravelTime = 3f; // Maximum time to travel on the Y-axis
    public float minZTravelTime = 1f; // Minimum time to travel on the Z-axis
    public float maxZTravelTime = 3f; // Maximum time to travel on the Z-axis

    private float currentXTravelTime; // Current travel time on the X-axis
    private float currentYTravelTime; // Current travel time on the Y-axis
    private float currentZTravelTime; // Current travel time on the Z-axis
    private Vector3 startingPosition; // Starting position of the object
    private Vector3 targetPosition; // Target position for each leg of the movement
    private bool isMovingForward = true; // Flag to track movement direction

    private void Start()
    {
        // Store the starting position
        startingPosition = transform.localPosition;

        // Randomize the travel times for each axis
        currentXTravelTime = Random.Range(minXTravelTime, maxXTravelTime);
        currentYTravelTime = Random.Range(minYTravelTime, maxYTravelTime);
        currentZTravelTime = Random.Range(minZTravelTime, maxZTravelTime);

        // Set the initial target position to point B
        targetPosition = pointB;
    }

    private void Update()
    {
        // Calculate the movement speeds based on the travel times
        float movementSpeedX = Vector3.Distance(pointA, pointB) / currentXTravelTime;
        float movementSpeedY = Vector3.Distance(pointA, pointB) / currentYTravelTime;
        float movementSpeedZ = Vector3.Distance(pointA, pointB) / currentZTravelTime;

        // Move the object towards the target position
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, movementSpeedX * Time.deltaTime);

        // Check if the object has reached the target position
        if (transform.localPosition == targetPosition)
        {
            // Toggle the movement direction
            isMovingForward = !isMovingForward;

            // Set the new target position based on the movement direction
            if (isMovingForward)
            {
                targetPosition = pointB;
            }
            else
            {
                targetPosition = pointA;

                // Randomize the travel times for each axis
                currentXTravelTime = Random.Range(minXTravelTime, maxXTravelTime);
                currentYTravelTime = Random.Range(minYTravelTime, maxYTravelTime);
                currentZTravelTime = Random.Range(minZTravelTime, maxZTravelTime);
            }
        }
    }
    }
}