using System.Collections.Generic;
using UnityEngine;

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

    public enum OBSTACLE_TYPE {CLOUD_1, CLOUD_2, EAGLE};

    public GameObject[] _posibleObstacles = new GameObject[System.Enum.GetNames(typeof(OBSTACLE_TYPE)).Length];

    public BallMovement _player;
    public GUIManager GUIManager;
    public List<GameObject> _obstacles;
    
    float height = 0;
    bool _obstacleInstantiated = false;

    private void Start()
    {
        _player = FindObjectOfType<BallMovement>();
        GUIManager = FindObjectOfType<GUIManager>();
        string[] obstaclesTypes = System.Enum.GetNames(typeof(OBSTACLE_TYPE));
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
            if (aux >= height && aux - height > OBSTACLES_INTER_DISTANCE)
            {
                height = aux;
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

        bool toRight = Random.Range(0, 1.0f) >= 0.5f;
        GameObject objectToInstantiate = _posibleObstacles[Random.Range(0, _posibleObstacles.Length)];
        Vector3 position = Vector3.zero;
        position.y = height;
        position.x = objectToInstantiate.GetComponent<ObstacleMovement>()._spawnPosition * (toRight ? -1 : 1);
        GameObject go = Instantiate(objectToInstantiate, position, Quaternion.identity);
        go.GetComponent<ObstacleMovement>()._rightInitialDirection = toRight;
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
    }
}
