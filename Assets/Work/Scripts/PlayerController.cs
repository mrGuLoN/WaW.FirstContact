using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Joystick _movementJoystick, _fireJoystick;
    [SerializeField] private float _speed;

    private CharacterController _characterController;
    private Transform _thisTR;
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _thisTR = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_movementJoystick.Horizontal != 0 || _movementJoystick.Vertical != 0)
        {
            _thisTR.forward = new Vector3(_movementJoystick.Horizontal, 0, _movementJoystick.Vertical);
            _animator.SetFloat("Input",1);
            _characterController.Move(_thisTR.forward * _speed * Time.deltaTime);
        }
        else
        {
            _animator.SetFloat("Input",0);
        }
    }
}
