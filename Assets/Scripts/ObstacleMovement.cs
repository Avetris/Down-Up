using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{

    public float _spawnPosition = 8.0f;
    public float _speed = 0.0f;
    public bool _rightInitialDirection = true;
    private Animator _animator;

    private void Start()
    {
        if (!_rightInitialDirection)
        {
            _speed *= -1;
        }
        _animator = GetComponent<Animator>();
        checkAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.right * _speed * Time.deltaTime, Space.World);
        if(Mathf.Abs(this.transform.localPosition.x) > _spawnPosition)
        {
            _speed = Mathf.Abs(_speed) * (this.transform.localPosition.x > 0 ? -1 : 1) ;
            checkAnimation();
        }
    }

    private void checkAnimation()
    {
        if(_animator != null)
        {
            _animator.SetFloat("Speed", _speed);
        }
    }
}
