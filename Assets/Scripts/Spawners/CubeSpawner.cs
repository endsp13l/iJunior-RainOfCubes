using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private int _poolCapacity = 15;
    [SerializeField] private int _poolMaxSize = 30;
    [SerializeField] private float _spawnDelay = 0.25f;

    [Header("SpawnAreaBorders")] [SerializeField]
    private Transform _spawnHeight;

    [SerializeField] private Transform _leftBorder;
    [SerializeField] private Transform _rightBorder;
    [SerializeField] private Transform _frontBorder;
    [SerializeField] private Transform _backBorder;

    [SerializeField] private BombSpawner _bombSpawner;

    private Pool<Cube> _cubePool;
    private int _spawnedCubesCount;

    private Coroutine _coroutine;
    private bool _isRunning;

    private void Awake()
    {
        _cubePool = new Pool<Cube>(_cubePrefab, _poolCapacity, _poolMaxSize);
        _cubePool.Initialize();
    }

    private void OnEnable()
    {
        _isRunning = true;
        _cubePool.ObjectReleased += _bombSpawner.Spawn;
    }

    private void Start() => _coroutine = StartCoroutine(Spawn());

    private void OnDisable()
    {
        _isRunning = false;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _cubePool.ObjectReleased -= _bombSpawner.Spawn;
    }

    private IEnumerator Spawn()
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnDelay);

        while (_isRunning)
        {
            yield return wait;
            _cubePool.Get().transform.position = GetRandomPosition();
        }
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(_frontBorder.position.x, _backBorder.position.x);
        float y = _spawnHeight.position.y;
        float z = Random.Range(_leftBorder.position.z, _rightBorder.position.z);

        return new Vector3(x, y, z);
    }
}