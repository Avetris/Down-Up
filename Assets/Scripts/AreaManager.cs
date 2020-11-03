using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class AreaManager : MonoBehaviour
{
    #region SINGLETON
    private static AreaManager _instance;

    public static AreaManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject auxiliarObject = new GameObject("AreaManager");
                _instance = auxiliarObject.AddComponent<AreaManager>();
            }

            return _instance;
        }
    }

    #endregion

    public int OBSTACLE_COOLDOWN = 10;
    public int OBSTACLES_INTER_DISTANCE = 10;


    public GameObject[] _posibleObstacles = new GameObject[System.Enum.GetNames(typeof(ObstacleType)).Length];

    public BallMovement _player;
    public GUIManager GUIManager;
    public List<GameObject> _obstacles;
    
    float _height = 0;
    bool _obstacleInstantiated = false;

    private void Start()
    {
        _player = FindObjectOfType<BallMovement>();
        GUIManager = FindObjectOfType<GUIManager>();
        string[] obstaclesTypes = System.Enum.GetNames(typeof(ObstacleType));
        for (int i = 0; i < obstaclesTypes.Length; i++)
        {
            _posibleObstacles[i] = Resources.Load<GameObject>("Prefabs/"+ obstaclesTypes[i]);
        }
        _obstacles = new List<GameObject>();

    }

    private void Update()
    {
        if (!GUIManager.IsStopped() && !_obstacleInstantiated && _player._rigidbody.velocity.y > 0)
        {            
            float aux = _player.transform.localPosition.y + (_player._rigidbody.velocity.y);
            if (aux >= _height && aux - _height > OBSTACLES_INTER_DISTANCE)
            {
                _height = aux;
                InstantiateObstacle();
            }
        }
    }

    private void ResetObstacleTimeout()
    {
        _obstacleInstantiated = false;
    }

    private void InstantiateObstacle()
    {
        _obstacleInstantiated = true;

        if (_height <= (_player.transform.position.y + 20))
        {
            _height = _player.transform.position.y + 20;
        }

        GameObject objectToInstantiate = _posibleObstacles[Random.Range(0, _posibleObstacles.Length)];
        Vector3 position = Vector3.zero;
        position.y = _height;
        GameObject go = Instantiate(objectToInstantiate, position, Quaternion.identity);
        _obstacles.Add(go);

        Invoke("ResetObstacleTimeout", OBSTACLE_COOLDOWN);
    }

    public void Restart()
    {
        foreach(GameObject go in _obstacles)
        {
            if(go != null)
            {
                Destroy(go);
            }
        }
        _obstacles.Clear();
        _height = 0;
    }
}
