using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class BallMovement : MonoBehaviour
{
    public GameObject _trampolineGO;
    public Vector3 _initialPosition = new Vector3(0, 0, 0);

    [HideInInspector]
    GUIManager _GUIManager;
    [HideInInspector]
    public AreaManager _areaManager;
    [HideInInspector]
    public Rigidbody2D _rigidbody;
    int _maxHeight = 0;

    Vector2 _firstPressPos;
    bool _swipeStarted = false;

    TrailRenderer _trailRenderer;

    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _GUIManager = FindObjectOfType<GUIManager>();
        _trailRenderer = GetComponent<TrailRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _areaManager = AreaManager.Instance;
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_GUIManager.IsStopped())
        {
            transform.Rotate(new Vector3(_rigidbody.velocity.y, _rigidbody.velocity.x, 0));
            if (this.transform.localPosition.y > _maxHeight)
            {
                _maxHeight = Mathf.FloorToInt(this.transform.localPosition.y);
                _GUIManager.SetScore(_maxHeight);                
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

    private void FixedUpdate()
    {
        
    }

#if UNITY_EDITOR
    public void SwipeMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            _firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            _swipeStarted = true;
        }
        if (Input.GetMouseButtonUp(0) && _swipeStarted)
        {
            //create vector from the two points
            Vector2 currentSwipe = new Vector2(Input.mousePosition.x - _firstPressPos.x, Input.mousePosition.y - _firstPressPos.y);

            //normalize the 2d vector
            currentSwipe.Normalize();
            _swipeStarted = false;

            if (currentSwipe.x != 0)
                Move(currentSwipe.x > 0);
        }
    }
#else
    public void SwipeMobile()
    {        
        if(Input.touchCount > 0)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began){
                _firstPressPos = Input.GetTouch(0).position;
                _swipeStarted = true;
            }else if(Input.GetTouch(0).phase == TouchPhase.Moved && _swipeStarted){
                Vector2 currentSwipe = Input.GetTouch(0).deltaPosition;
                _swipeStarted = false;

                //normalize the 2d vector
                currentSwipe.Normalize();

                if (currentSwipe.x != 0)
                    Move(currentSwipe.x > 0);
            }
        }
    }
#endif
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(_rigidbody.velocity);
        if ("trampoline".Equals(collision.gameObject.tag))
        {
            _audioSource.Play();
            _rigidbody.AddForce(_rigidbody.velocity * REPULSION_FORCE, ForceMode2D.Impulse);
            //_rigidbody.velocity = _rigidbody.velocity + _rigidbody.velocity.normalized * REPULSION_FORCE;
        }else if("obstacle".Equals(collision.gameObject.tag))
        {
            EndGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ("end_object".Equals(collision.gameObject.tag))
        {
            EndGame();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ("cloud".Equals(collision.gameObject.tag))
        {
            _rigidbody.velocity *= CLOUD_BRAKE;
        }
        else if ("vortex".Equals(collision.gameObject.tag))
        {
            float xValue = collision.transform.position.x;
            if (transform.position.x > xValue)
            {
                transform.Translate(Vector3.left * Time.deltaTime);
            }
            else if (transform.position.x < xValue)
            {
                transform.Translate(Vector3.right * Time.deltaTime);
            }
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
        _rigidbody.AddForce(direction * MOVEMENT_FORCE, ForceMode2D.Impulse);
    }

    public void StartGame()
    {
        _rigidbody.gravityScale = 1;
    }

    public void ResetGame()
    {
        _maxHeight = 0;
        _swipeStarted = false;
        _rigidbody.gravityScale = 0;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.freezeRotation = false;
        _trailRenderer.enabled = false;
        this.transform.position = _initialPosition;
        this.transform.rotation = Quaternion.Euler(0,0,0);
        _trailRenderer.enabled = true;
    }
    private void EndGame()
    {
        _rigidbody.gravityScale = 0;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.freezeRotation = true;
        _areaManager.Restart();
        _GUIManager.ShowGameOver(_maxHeight);
    }
}
