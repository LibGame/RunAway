using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent (typeof(CharacterController))]

public class PlayerController : Player
{

    private Vector3 _moveDirection = Vector3.zero;
    private CharacterController _characterController;
    [SerializeField] private Interface _interface;
    [SerializeField] protected Joystick _input;
    [HideInInspector] public float DefaulSpeed;
    [HideInInspector] public bool IsJump;
    private float nextTime = 0f;
    private float timeRate = 0.5f;
    private float xPos;
    private float yPos;
    public bool IsMobileVersion; // Если мобильная версия то использовать ли джостики ?
    private bool _isInFly; // если в полете

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        DefaulSpeed = _moveSpeed;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SpeedUp();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            SpeedDown();
        }
    }

    private void FixedUpdate()
    {
        if (_characterController.isGrounded)
        {
            _isInFly = false;
            if (IsMobileVersion)
            {
                xPos = _input.HorizontalPosition;
                yPos = _input.VerticalPosition;
            }
            else
            {
                xPos = Input.GetAxis("Horizontal");
                yPos = Input.GetAxis("Vertical");
            }

            _moveDirection = new Vector3(xPos, 0, yPos);
            _moveDirection = transform.TransformDirection(_moveDirection);
            _moveDirection *= MoveSpeed;
        }

        if((Input.GetKeyDown(KeyCode.Space) || IsJump) && _isInFly != true)
        {
            IsJump = false;
            _isInFly = true;
            Jump();
        }

        _moveDirection.y -= Gravity * Time.deltaTime;

        _characterController.Move(_moveDirection * Time.deltaTime);

    }

    public void Jump()
    {
        _moveDirection.y = JumpForce;
    }

    // проверяем на сталкновение с мечом
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword" && Time.time > nextTime)
        {
            GetDamage(30);
            _interface.DispaleyStatsHealth(Health);
            nextTime = Time.time + timeRate;
        }
    }

    // получаем урон
    private void GetDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
        {        
            PlayerDeath();
        }
    }

    // сичтаем и отображаем усталость
    public void StartDisplayTires()
    {
        _interface.StartTires(1, 0.05f);
    }

    // перестаем считать и отображать усталость
    public void StopDisplayTires()
    {
        _interface.StopTires();
    }


    // востанавливаем усталость
    public void FatigueRecoveryTires()
    {
        _interface.InvokeRecovery(2f);
    }

    // игрок умер
    public void PlayerDeath()
    {
        _interface.ShowLoseTable();
    }

    public void SpeedUp()
    {
        if (Fatigue > 0)
        {
            MoveSpeed *= 2f;
            StartDisplayTires();
        }
    }

    public void SpeedDown()
    {
        MoveSpeed = DefaulSpeed;
        StopDisplayTires();
        FatigueRecoveryTires();
    }
    public IEnumerator LadderMoveUp()
    {
        while (true)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 5f);

            yield return null;
        }

    }

}
