using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _changingSpeedTime;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _sittingSpeed;
    [SerializeField] private float _startTackleSpeed;
    [SerializeField] private float _tackleSlowdown;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _fallSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private CameraEffector _effector;
    [SerializeField] private CharacterController _controller;

    private MoveCondition _moveCondition;
    private float _currentSpeed;
    private float _targetSpeed;
    private Vector3 _planeMoving;
    private float _yMoving;

    public void TryJump()
    {
        if(_controller.isGrounded)
            _yMoving = _jumpForce;
    }

    public void Move(Vector3 direction)
    {
        if (_moveCondition == MoveCondition.Stand)
            TryWalk();
        _planeMoving = direction;
        _currentSpeed = Mathf.Lerp(_currentSpeed, _targetSpeed, _changingSpeedTime);
    }

    private void Update()
    {
        if (!_controller.isGrounded)
            _yMoving -= _fallSpeed * Time.deltaTime;
        _controller.Move((_planeMoving * _currentSpeed + transform.up * _yMoving) * Time.deltaTime);
    }

    public void TryRunning()
    {
        if (_moveCondition == MoveCondition.Walk)
        {
            _moveCondition = MoveCondition.Run;
            _targetSpeed = _runSpeed;
            _effector.RunEffect();
        }
    }

    public void TrySit()
    {
        if (_moveCondition == MoveCondition.Run)
            StartCoroutine(StartTackle());
        else if (_moveCondition == MoveCondition.Walk)
        {
            _moveCondition = MoveCondition.Sit;
            _targetSpeed = _sittingSpeed;
        }
    }

    public void TryWalk()
    {
        if(_moveCondition == MoveCondition.Stand || _moveCondition == MoveCondition.Run || _moveCondition == MoveCondition.Sit)
        {
            _moveCondition = MoveCondition.Walk;
            _targetSpeed = _walkSpeed;
            _effector.WalkEffect();
        }
    }

    public void Stand()
    {
        _moveCondition = MoveCondition.Stand;
        _targetSpeed = 0;
        _effector.StandEffect();
    }

    public void Rotate(Vector3 axis, float speed)
    {
        transform.Rotate(axis, speed * _rotationSpeed, Space.World);
    }

    private IEnumerator StartTackle()
    {
        _moveCondition = MoveCondition.Tackle;
        _currentSpeed = _startTackleSpeed;
        _targetSpeed = _startTackleSpeed;
        _effector.TackleEffect();
        while (_targetSpeed > _sittingSpeed)
        {
            _targetSpeed -= _tackleSlowdown * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        _moveCondition = MoveCondition.Sit;
        TryWalk();
    }

    private void Start()
    {
        _moveCondition = MoveCondition.Stand;
        _targetSpeed = 0;
    }
}

public enum MoveCondition
{
    Stand,
    Walk,
    Run,
    Sit,
    Tackle
}