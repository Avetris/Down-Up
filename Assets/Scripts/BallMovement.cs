using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [Header("Repulsion Force")]
    public float REPULSION_FORCE = 10f;
    public float MOVEMENT_FORCE = 100.0f;

    public GameObject _trampolineGO;
    public Vector3 _initialPosition = new Vector3(0, 0, 0);

    public GUIManager GUIManager;

    Rigidbody _rigidbody;
    int _maxHeight = 0;

    #if UNITY_EDITOR
        //inside class
        Vector2 firstPressPos;
        Vector2 secondPressPos;
        Vector2 currentSwipe;
    #endif

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GUIManager.IsStopped())
        {
            if (this.transform.localPosition.y > _maxHeight)
            {
                _maxHeight = Mathf.FloorToInt(this.transform.localPosition.y);
                GUIManager.SetScore(_maxHeight);                
            }
            else if (this.transform.localPosition.y < _trampolineGO.transform.localPosition.y)
            {
                EndGame();
            }

#if UNITY_EDITOR
            SwipeMouse();
#else
            SwipeMobile();
#endif

        }
    }

#if UNITY_EDITOR
    public void SwipeMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetMouseButtonUp(0))
        {
            //save ended touch 2d point
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //create vector from the two points
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

            //normalize the 2d vector
            currentSwipe.Normalize();

            if (currentSwipe.x != 0)
                Move(currentSwipe.x > 0);
        }
    }
#else
    public void SwipeMobile()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {

            // Get movement of the finger since last frame
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

            if (touchDeltaPosition.x != 0)
                Move(touchDeltaPosition.x > 0);
        }
    }
#endif

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log(_rigidbody.velocity);
        if ("trampoline".Equals(collision.gameObject.tag))
        {
            _rigidbody.AddForce(_rigidbody.velocity * REPULSION_FORCE, ForceMode.Impulse);
            //_rigidbody.velocity = _rigidbody.velocity + _rigidbody.velocity.normalized * REPULSION_FORCE;
        }else if ("lava".Equals(collision.gameObject.tag))
        {
            EndGame();
        }
    }

    private void Move(bool moveRight)
    {
        Vector3 direction = Vector3.zero;
        if (moveRight)
        {
            direction = Vector3.right;
        }
        else
        {
            direction = Vector3.left;
        }
        _rigidbody.AddForce(direction * MOVEMENT_FORCE, ForceMode.Impulse);
    }

    public void StartGame()
    {
        _rigidbody.useGravity = true;
    }

    public void ResetGame()
    {
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.freezeRotation = false;
        this.transform.position = _initialPosition;
    }
    private void EndGame()
    {
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.freezeRotation = true;
        GUIManager.ShowGameOver(_maxHeight);
    }
}
