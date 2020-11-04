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
    private Dictionary<ObstacleType, GameObject> _posibleObstacles = new Dictionary<ObstacleType, GameObject>();

    public BallMovement _player;
    public GUIManager GUIManager;
    public List<GameObject> _obstacles;
    
    float _height = 0;
    bool _obstacleInstantiated = false;

    private void Start()
    {
        _player = FindObjectOfType<BallMovement>();
        GUIManager = FindObjectOfType<GUIManager>();
        foreach ((ObstacleType obstacle, int value)  in OBSTACLE_PROBABILITY)
        {
            _posibleObstacles.Add(obstacle, Resources.Load<GameObject>("Prefabs/"+ obstacle.ToString("G")));
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

        GameObject objectToInstantiate = GetRandomObstacle();
        Vector3 position = Vector3.zero;
        position.y = _height;
        GameObject go = Instantiate(objectToInstantiate, position, Quaternion.identity);
        _obstacles.Add(go);

        ResetObstacleTimeout();
    }

    private GameObject GetRandomObstacle() {
        float randomValue = Random.Range(0, OBSTACLE_PROBABILITY[OBSTACLE_PROBABILITY.Length - 1].probability);
        GameObject objectToInstantiate = null;

        foreach ((ObstacleType obstacle, int value) in OBSTACLE_PROBABILITY)
        {
            if (randomValue <= value)
            {
                objectToInstantiate = _posibleObstacles[obstacle];
                break;
            }
        }

        return objectToInstantiate;
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
