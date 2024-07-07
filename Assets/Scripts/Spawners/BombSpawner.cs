using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private int _poolCapacity = 15;
    [SerializeField] private int _poolMaxSize = 30;
    [SerializeField] private float _spawnDelay = 0.25f;

    [Header("SpawnAreaBorders")] 
    [SerializeField] private Transform _spawnHeight;
    [SerializeField] private Transform _leftBorder;
    [SerializeField] private Transform _rightBorder;
    [SerializeField] private Transform _frontBorder;
    [SerializeField] private Transform _backBorder;

    private Spawner<Bomb> _spawner;
    private Coroutine _coroutine;
    private bool _isRunning;
    
   private void Awake()
    {
       _spawner = new Spawner<Bomb>(_bombPrefab, _poolCapacity, _poolMaxSize);
       _spawner.Initialize();
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
            var bomb = _spawner.ObjectPool.Get();

            bomb.transform.position = GetRandomPosition();
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