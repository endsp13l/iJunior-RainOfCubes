using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private int _poolCapacity = 15;
    [SerializeField] private int _poolMaxSize = 30;
    [SerializeField] private float _spawnDelay = 0.25f;

    [Header("SpawnAreaBorders")] 
    [SerializeField] private Transform _spawnHeight;
    [SerializeField] private Transform _leftBorder;
    [SerializeField] private Transform _rightBorder;
    [SerializeField] private Transform _frontBorder;
    [SerializeField] private Transform _backBorder;

    private ObjectPool<GameObject> _cubePool;

    private Coroutine _coroutine;
    private bool _isRunning;

    private void Awake()
    {
        _cubePool = new ObjectPool<GameObject>(
            createFunc: () => CreateCube(),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: ActionOnDestroy,
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void OnEnable() => _isRunning = true;

    private void Start() => _coroutine = StartCoroutine(Spawn());

    private void OnDisable()
    {
        _isRunning = false;

        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator Spawn()
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnDelay);

        while (_isRunning)
        {
            yield return wait;
            _cubePool.Get();
        }
    }

    private GameObject CreateCube()
    {
        Cube cube = Instantiate(_cubePrefab);
        cube.Destroyed += _cubePool.Release;

        return cube.gameObject;
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = GetRandomPosition();
        obj.gameObject.SetActive(true);
    }

    private void ActionOnDestroy(GameObject obj)
    {
        Cube cube = obj.GetComponent<Cube>();

        cube.Destroyed -= _cubePool.Release;
        Destroy(obj);
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(_frontBorder.position.x, _backBorder.position.x);
        float y = _spawnHeight.position.y;
        float z = Random.Range(_leftBorder.position.z, _rightBorder.position.z);

        return new Vector3(x, y, z);
    }
}