using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class ObstacleMovement : MonoBehaviour
{
    public ObstaclesMovement _obstacleMovement;
    public Vector3[] _spawnPositions = new Vector3[0];
    public Vector3[] _endPositions = new Vector3[0];
    public Vector3 _rotationVector = Vector3.zero;
    public float _stepSpeed = 1.0f;

    public ParticleSystem _particleSystem;
    public bool _particleStopInNegative = true;
    public bool[] _particleAxis = new bool[2] { false, false };

    private Animator _animator;

    private Vector3[] _currentPos = new Vector3[2];
    private bool _waitingForSpawn = false;


    private void Start()
    {
        Initialize();
        _animator = GetComponent<Animator>();
        CheckAnimation();
    }

    public void Initialize()
    {
        int selectedPosition = Random.Range(0, _spawnPositions.Length);
        if(_endPositions.Length > 0)
        {
            _currentPos = new Vector3[2] { _spawnPositions[selectedPosition], _endPositions[selectedPosition] };

            _currentPos[0].z = transform.position.z;
            _currentPos[1].z = transform.position.z;

            _currentPos[0].y += transform.position.y;
            _currentPos[1].y += transform.position.y;
        }
        else
        {
            _currentPos = new Vector3[1] { _spawnPositions[selectedPosition]};

            _currentPos[0].z = transform.position.z;
            _currentPos[0].y += transform.position.y;

            if(_rotationVector != Vector3.zero)
            {
                if (_currentPos[0].x < 0)
                    _rotationVector *= -1;
                transform.rotation = Quaternion.Euler(_rotationVector);
            }
        }

        transform.position = _currentPos[0];

        CheckAnimation();
        _waitingForSpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_waitingForSpawn && _currentPos.Length > 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentPos[1], _stepSpeed);
            if (Vector3.Distance(transform.position, _currentPos[1]) < 0.001f)
            {
                if (_obstacleMovement == ObstaclesMovement.ONE_DIMENSION)
                {
                    SwitchDirection();
                    CheckAnimation();
                }
                else if (_obstacleMovement == ObstaclesMovement.TWO_DIMENSIONS)
                {
                    _waitingForSpawn = true;
                    transform.position = _currentPos[0];
                    Invoke("Initialize", 1f);

                }
            }
        }
    }

    private void SwitchDirection()
    {
        Vector3 aux = _currentPos[0];
        _currentPos[0] = _currentPos[1];
        _currentPos[1] = aux;
    }

    private void CheckAnimation()
    {
        float speedX = _currentPos[1].x - transform.position.x;
        float speedY = _currentPos[1].y - transform.position.y;
        if (_animator != null)
        {            
            if(speedX != 0) _animator.SetFloat("SpeedX", speedX);
            if(speedY != 0) _animator.SetFloat("SpeedY", speedY);
        }
        else if (_particleSystem != null)
        {
            if ((_particleAxis[0] && ((_particleStopInNegative && speedX < 0) || (!_particleStopInNegative && speedX > 0))) ||
                (_particleAxis[1] && ((_particleStopInNegative && speedY < 0) || (!_particleStopInNegative && speedY > 0)))) {
                _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
            else if ((_particleAxis[0] && ((_particleStopInNegative && speedX > 0) || (!_particleStopInNegative && speedX < 0))) ||
                (_particleAxis[1] && ((_particleStopInNegative && speedY > 0) || (!_particleStopInNegative && speedY < 0)))) {
                _particleSystem.Play(true);
            }
        }
    }
}
