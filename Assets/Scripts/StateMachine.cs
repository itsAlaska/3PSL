using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private State _currentState;
    
    void Start()
    {
        
    }
    
    void Update()
    {   
        // Updating the current states animation by calling the Tick method in State.cs
        _currentState?.Tick(Time.deltaTime);
    }

    // Method to switch between states, requires that the state being switched to be passed in to the method
    public void SwitchState(State newState)
    {
        // Exits current state
        _currentState?.Exit();
        // Changes _currentState to be the new state passed in the method
        _currentState = newState;
        // Initializes the _currentState variable in the new state 
        _currentState?.Enter();
    }
}
