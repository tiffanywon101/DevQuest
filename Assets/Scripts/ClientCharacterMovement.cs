using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClientCharacterMovement : MonoBehaviour
{
    private InputAction _movePosition;
    private InputAction _moveAction;
    private bool _move = false;
    private Vector3 _targetPos;
    private float _distanceCheck = 0.1f;
    [SerializeField] // visible in editor
    private float _walkSpeed = 1.0f;

    void Start()
    {
        //Player Input setup
        _targetPos = transform.position;
        var playerInput = GetComponent<PlayerInput>();
        // get actions from the Input Action bound to Player Input.
        _movePosition = playerInput.actions["LocoTarget"];
        _moveAction = playerInput.actions["MoveTarget"];
        
    }

    //we will check for user input every frame
    private void Update()
    {
        ProcessInput();
        _move = MoveToPosition(_targetPos);
    }
    private bool MoveToPosition(Vector3 _targetPos)
    {
        if (Vector3.Magnitude(transform.position - _targetPos) < _distanceCheck)
        {
            // arrived. cancel move.
            Debug.Log("arrived! stop moving!");
            return false;
        }
        // should move towards target
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, Time.deltaTime * _walkSpeed);
        transform.LookAt(_targetPos, Vector3.up);
        return true;
    }

    // processes user input
    private void ProcessInput()
    {
        // RMB pressed
        if (_moveAction.triggered)
        {
            // mouse position
            Vector2 screenPos = _movePosition.ReadValue<Vector2>();
            // raycast to the ground, to get the position on the ground to move our character to.
            var ray = Camera.main.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out RaycastHit hit, 500.0f, LayerMask.GetMask("Ground")))
            {
                //Let's just debug the input for now.
                _targetPos = hit.point;
                //Debug.Log(hit.point);
            }
        }
    }

}
