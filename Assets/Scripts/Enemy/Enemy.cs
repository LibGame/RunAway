using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    protected enum TypeOfBehavior
    {
        Atack = 0,
        MoveTrajectory = 1,
        FollowPlayer = 2,
        LookAround = 3,
        LookAtHouse = 4,
        MoveToLastPosition = 5,

    }

    [SerializeField] protected TypeOfBehavior _typeOfBehavior = TypeOfBehavior.MoveTrajectory;
    [SerializeField] protected GameObject _house;
    [SerializeField] protected Transform _lastPositionPoint;
    protected CharacterController _characterController;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _damage;
    public float Damage { get; set;  }

    [Header("Точки по которым будет двигатся враг")]

    [SerializeField] protected List<Transform> _points = new List<Transform>();
    [SerializeField] protected int pointID = 0;

    [Header("Область видимости врага")]

    [SerializeField] protected int _rayAmount;
    [SerializeField] protected float _rayDistance;
    [SerializeField] protected float _angle;
    [SerializeField] protected Vector3 _offset;
    private bool _isAtackAnimation; // идет ли сейчас анимация атаки ?
    private float _timer;
    protected Vector3 _playerLastPosition;


    // для возможности обходить препядствия

    private RaycastHit Hit;
    protected Transform _transformSelf;
    private int _distanceRayConfront = 3;


    // Двигаемся по траектории от точки к точки
    protected void MoveTrajectory(Animation anim, string animName , float time)
    {
        anim.Play(animName);
        GoAroundCollider(new Vector3(_points[pointID].position.x, _points[pointID].position.y, _points[pointID].position.z));

        if (_timer >= time)
        {
            _timer = 0;
            _typeOfBehavior = TypeOfBehavior.LookAtHouse;
        }

        _timer = _timer + 1 * Time.deltaTime;
    }


    // проверяем коснулся ли игрок точки если да то посылаем его к следующей точки
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Point")
        {
            if (pointID == _points.Count - 1)
            {
                pointID = 0;
            }
            else
            {
                pointID += 1;
            }
        }

        if(other.gameObject.tag == "LastPositionPoint") // если дошол до последней точки
        {
            _typeOfBehavior = TypeOfBehavior.LookAround;
        }
    }

    // Смотрит в сторону дома
    public void LookAtHouse(GameObject _player, Animation anim, string animName)
    {
        LookAt(_house.transform.position);
        if (ScanViewAngle(_player))
        {
            _timer = 0;
            _typeOfBehavior = TypeOfBehavior.FollowPlayer;
        }
        if (_timer >= 3)
        {
            _timer = 0;
            _typeOfBehavior = TypeOfBehavior.MoveTrajectory;
        }

        _timer = _timer + 1 * Time.deltaTime;
    }

    // Создаю два райкаста 1 один отзеркаливаю от другого , для того чтобы сделать поле зрение врага
    protected bool ScanViewAngle(GameObject _player)
    {
        RaycastHit hit1 = new RaycastHit();
        RaycastHit hit2 = new RaycastHit();
        Vector3 pos = transform.position + _offset;
        float angleBetwen = 0f;
        float angleBetwen1 = 0f;
        Vector3 direction = new Vector3(0, 0, 0);
        Vector3 direction1 = new Vector3(0, 0, 0);

        for (int i = 0; i < _rayAmount; i++)
        {
            var x = Mathf.Sin(angleBetwen);
            var y = Mathf.Cos(angleBetwen);
            var x1 = Mathf.Sin(angleBetwen1);
            var y1 = Mathf.Cos(angleBetwen1);
            angleBetwen += _angle * Mathf.Deg2Rad / _rayAmount;
            angleBetwen1 -= _angle * Mathf.Deg2Rad / _rayAmount;
            direction = transform.TransformDirection(new Vector3(x, 0, y));
            direction1 = transform.TransformDirection(new Vector3(x1, 0, y1));

            if (Physics.Raycast(pos, direction, out hit1, _rayDistance))
            {
                if (hit1.transform == _player.transform)
                {
                    _timer = 0;
                    return true;
                }
            }

            if (Physics.Raycast(pos, direction1, out hit2, _rayDistance))
            {
                if (hit2.transform == _player.transform)
                {
                    _timer = 0;
                    return true;
                }
            } 

        }


        return false;
    }

    // обходит стены
    private void GoAroundCollider(Vector3 target)
    {
        Vector3 angel = (target - _transformSelf.position).normalized;

        if (Physics.Raycast(_transformSelf.position,transform.forward, out Hit, 2))
        {
            angel += Hit.normal * 5;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(angel), Time.deltaTime * 4f);
        _characterController.Move(transform.forward * _moveSpeed * Time.deltaTime);
        _characterController.Move(Vector3.down * 9.8f * Time.deltaTime);

    }

    // Враг следует за игроком пока не упустит его из виду
    protected void ChasePlayer(GameObject _player, Animation anim ,string animName)
    {
        LookAt(_player.transform.position);

        if (CheckDistance(_player.transform.position) <= 2f)
        {
            HitPlayer(anim, "Atack");
            _isAtackAnimation = true;
        }
        else
        {
            GoAroundCollider(_player.transform.position);
            //transform.position = Vector3.MoveTowards(transform.position, new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z), _moveSpeed * Time.deltaTime);
            if (_isAtackAnimation != true)
            {
                anim.Play(animName);
            }

        }
        // Перестает преследовать игрока если расстояние между игроком больше 15 , или если не видел игрока больше 4 секунд

        if (CheckDistance(_player.transform.position) >= 25f || (!(ScanViewAngle(_player)) && _timer >= 4))
        {
            _typeOfBehavior = TypeOfBehavior.MoveToLastPosition;
            _lastPositionPoint.position = _playerLastPosition;
        }
        else
        {
            _playerLastPosition = _player.transform.position;
        }

        _timer = _timer + 1 * Time.deltaTime;
    }

    // если последний раз осматривает тереторию
    public void IsPlayerClose()
    {
        _typeOfBehavior = TypeOfBehavior.MoveTrajectory;
    }


    // Враг начинает бить игрока
    protected void HitPlayer(Animation anim, string animName)
    {
        anim.Play(animName);
    }

    // возвращяется к приследованию
    public void BackToChase()
    {
        _isAtackAnimation = false;
        _typeOfBehavior = TypeOfBehavior.FollowPlayer;
    }

    // Враг осматриваеться
    protected void LookAround(GameObject _player,Animation anim, string animName)
    {
        transform.Rotate(0, 150 * Time.deltaTime, 0);
        anim.Play(animName);

        if (ScanViewAngle(_player))
        {
            _typeOfBehavior = TypeOfBehavior.FollowPlayer;
        }
    }



    //Двигаетсья к последней позиции где видел игрока
    protected void MoveToLastPosition(Animation anim, string animName)
    {

        anim.Play(animName);
        GoAroundCollider(_playerLastPosition);
    }


    // считаю дистанцию
    protected float CheckDistance(Vector3 target)
    {
        return Vector3.Distance(transform.position, target);
    }

    // здесь указываю куда будет смотреть скилет
    protected void LookAt(Vector3 target)
    {
        transform.LookAt(target, Vector3.up);
    }
}
