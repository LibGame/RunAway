using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(CharacterController))]

public class Skileton : Enemy
{

    public Animation _animations;
    private float _defaultSpeed; // скорость с которой враг начинает игру
    private float _runSpeed; // скорость при беге
    [SerializeField] private GameObject _player;

    private void Start()
    {
        _defaultSpeed = _moveSpeed;
        _runSpeed = _moveSpeed * 2f;
        _angle = _angle / 2; // делю угол в двое из наследуемого класса Enemy чтобы отзеркалить одну сторону от другой
        _transformSelf = GetComponent<Transform>();
        _characterController = GetComponent<CharacterController>();

        if (Random.Range(0,2) == 1) // с шансом 50 процентов меняем направление движения
        {
            _points.Reverse();
        }
    }

    private void Update()
    {
        if (_typeOfBehavior == TypeOfBehavior.MoveTrajectory)
        {
            MoveTrajectory(_animations, "Walk" , 10);
            _moveSpeed = _defaultSpeed;
            if (ScanViewAngle(_player))
            {
                _typeOfBehavior = TypeOfBehavior.FollowPlayer;
            }

        }
        else if (_typeOfBehavior == TypeOfBehavior.FollowPlayer)
        {
            _moveSpeed = _runSpeed;
            ChasePlayer(_player, _animations, "Run");
        }
        else if (_typeOfBehavior == TypeOfBehavior.LookAround)
        {
            LookAround(_player, _animations, "LookAround");
        }
        else if (_typeOfBehavior == TypeOfBehavior.MoveToLastPosition)
        {
            MoveToLastPosition(_animations, "Run");
            _moveSpeed = _runSpeed;

        }
        else if (_typeOfBehavior == TypeOfBehavior.LookAtHouse)
        {
            LookAtHouse(_player, _animations, "LookAround");
        }

    }
}
