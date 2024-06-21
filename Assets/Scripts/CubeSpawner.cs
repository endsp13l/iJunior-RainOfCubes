using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private int _poolCapacity = 50;
    [SerializeField] private int _poolMaxSize = 50;
    [SerializeField] private float _spawnDelay = 1f;

    private ObjectPool<Cube> _cubePool;

    private Coroutine _coroutine;
    private bool _isRunning;

    private void Awake()
    {
        _cubePool = new ObjectPool<Cube>(
            createFunc: () => CreateCube(),
            actionOnGet: ActionOnGet,
            actionOnRelease: cube => cube.gameObject.SetActive(false),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void OnEnable() => _isRunning = true;

    private void Start() => _coroutine = StartCoroutine(SpawnCubes());

    private void OnDisable()
    {
        _isRunning = false;
        
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private IEnumerator SpawnCubes()
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnDelay);

        while (_isRunning)
        {
            yield return wait;
            _cubePool.Get();
        }
    }

    private Cube CreateCube()
    {
        return Instantiate(_cubePrefab, _spawnPoint.position, Quaternion.identity);
    }

    private void ActionOnGet(Cube cube)
    {
        cube.gameObject.SetActive(true);
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(-5f, 5f);
        float y = Random.Range(-5f, 5f);
        float z = Random.Range(-5f, 5f);
        return new Vector3(x, y, z);
    }
}