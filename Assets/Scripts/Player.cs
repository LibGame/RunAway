
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Параметры персонажа")]
    private float _health = 100;
    private float _fatigue = 100;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _jumpForce;
    [SerializeField] protected float _gravity;
    protected float DefaulSpeed;
    public float MoveSpeed { get { return _moveSpeed; } set { if (value > 0) _moveSpeed = value; } }
    public float JumpForce { get { return _jumpForce; } set { if (value > 0) _jumpForce = value; } }
    public float Gravity { get { return _gravity; } set { if (value > 0) _gravity = value; } }
    public float Health { get { return _health; } set { if (_health >= 0) _health = value; } }
    public float Fatigue { get { return _fatigue; } set { if (value > 0) { _fatigue = value; } } }
}
