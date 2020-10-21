using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    float _leftPosition = -11.35f;
    Vector3 _initialPosition;

    private void Start()
    {
        _initialPosition = this.transform.position;
        _initialPosition.x = -1 * _leftPosition;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.left * Time.deltaTime, Space.Self);
        if (this.transform.position.x <= _leftPosition)
        {
            this.transform.position = _initialPosition;
        }
    }
}
