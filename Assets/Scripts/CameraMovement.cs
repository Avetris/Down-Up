using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject _ball;

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = this.transform.localPosition;
        if (_ball.transform.localPosition.y < 0)
        {
            currentPosition.y = 0;
        }
        else
        {
            currentPosition.y = _ball.transform.localPosition.y;
        }
        this.transform.localPosition = currentPosition;
    }
}
