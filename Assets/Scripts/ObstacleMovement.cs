using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class ObstacleMovement : MonoBehaviour
{
    public ObstaclesMovement _obstacleMovement;
    public Vector3[] _spawnPositions = new Vector3[0];
    public Vector3[] _endPositions = new Vector3[0];
    public float _stepSpeed = 1.0f;

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
        _currentPos = new Vector3[2] {_spawnPositions[selectedPosition],_endPositions[selectedPosition]};

        _currentPos[0].z = transform.position.z;
        _currentPos[1].z = transform.position.z;

        _currentPos[0].y += transform.position.y;
        _currentPos[1].y += transform.position.y;


        transform.position = _currentPos[0];

        CheckAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_waitingForSpawn)
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentPos[1], _stepSpeed);
            if (Vector3.Distance(transform.position, _currentPos[1]) < 0.001f)
            {
                if (_obstacleMovement == ObstaclesMovement.LEFT_RIGHT)
                {
                    SwitchDirection();
                    CheckAnimation();
                }
                else if (_obstacleMovement == ObstaclesMovement.DIAGONAL)
                {
                    _waitingForSpawn = true;
                    Invoke("MoveToInitial", 1f);

                }
            }
        }
    }

    private void MoveToInitial()
    {
        transform.position = _currentPos[0];
        _waitingForSpawn = false;
    }

    private void SwitchDirection()
    {
        Vector3 aux = _currentPos[0];
        _currentPos[0] = _currentPos[1];
        _currentPos[1] = aux;
    }

    private void CheckAnimation()
    {
        if(_animator != null)
        {
            float speedX = _currentPos[1].x - transform.position.x;
            float speedY = _currentPos[1].y - transform.position.y;
            
            if(speedX != 0) _animator.SetFloat("SpeedX", speedX);
            if(speedY != 0) _animator.SetFloat("SpeedY", speedY);
        }
    }
}
