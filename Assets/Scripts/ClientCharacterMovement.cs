using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClientCharacterMovement : MonoBehaviour
{
    private InputAction _movePosition;
    private InputAction _moveAction;
    private InputAction _jumpAction;

    private bool _move = false;
    private Vector3 _targetPos;
    private float _distanceCheck = 0.10f;

    [SerializeField]
    private float _walkSpeed = 1.0f;
    private Animator _anim;
    private CharacterController _controller;

    [SerializeField]
    private bool _grounded = true;
    void Start()
    {
        var playerInput = GetComponent<PlayerInput>();
        _movePosition = playerInput.actions["LocoTarget"];
        _moveAction = playerInput.actions["MoveTarget"];
        _jumpAction = playerInput.actions["Jump"];
        _targetPos = transform.position;
        _controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        ProcessInput();
        GroundCheck();
        bool moveResult = MoveToPosition(_targetPos);
        if (_move != moveResult)
        {
            _move = moveResult;
            _anim.SetBool("bWalk", _move);
        }

        _controller.Move(_horizontalVelocity * Time.deltaTime
            + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        // gravity
        if (!_grounded)
        {
            _verticalVelocity += _gravity * Time.deltaTime;
        }

        if (_grounded && _verticalVelocity < 0.0f)
        {
            //grounded
            _verticalVelocity = -0f;
        }
    }

    private void ProcessInput()
    {
        // RMB pressed
        if (_moveAction.triggered && _grounded)
        {
            // mouse position
            Vector2 screenPos = _movePosition.ReadValue<Vector2>();
            var ray = Camera.main.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out RaycastHit hit, 500.0f, LayerMask.GetMask("Ground")))
            {
                _targetPos = hit.point;
            }
        }
        if (_jumpAction.triggered && _grounded)
        {
            Jump();
        }

    }



    private void Jump()
    {
        _grounded = false;
        _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2.0f * _gravity);
    }

    [SerializeField]
    private float _gravity = -9.8f;
    [SerializeField]
    private float _jumpHeight = 1.0f;
    [SerializeField]
    private Transform _footTransform;
    private float _groundedRadius = 0.3f;

    private float _verticalVelocity = 0.0f;
    private Vector3 _horizontalVelocity = Vector3.zero;

    private void GroundCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = _footTransform.position;
        _grounded = Physics.CheckSphere(spherePosition, _groundedRadius, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore);
    }

    private void OnDrawGizmos()
    {
        if (_grounded) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;
        Vector3 spherePosition = _footTransform.position;
        Gizmos.DrawSphere(spherePosition, _groundedRadius);
    }
    private bool MoveToPosition(Vector3 _targetPos)
    {
        Vector3 groundTargetPos = new Vector3(_targetPos.x, 0.0f, _targetPos.z);
        Vector3 groundPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        if (Vector3.Magnitude(groundPos - groundTargetPos) < _distanceCheck)
        {
            // arrived. cancel move.
            _horizontalVelocity = Vector3.zero;
            return false;
        }
        // should move towards target
        //transform.position = Vector3.MoveTowards(transform.position, _targetPos, Time.deltaTime * _walkSpeed);
        _horizontalVelocity = (groundTargetPos - groundPos).normalized * _walkSpeed;
        transform.LookAt(transform.position + _horizontalVelocity, Vector3.up);
        return true;
    }

}