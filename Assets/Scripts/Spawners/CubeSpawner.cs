using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeSpawner : Spawner<Cube>
{
    [SerializeField] private float _spawnDelay = 0.5f;

    [Header("SpawnAreaBorders")] 
    [SerializeField] private Transform _spawnHeight;
    [SerializeField] private Transform _leftBorder;
    [SerializeField] private Transform _rightBorder;
    [SerializeField] private Transform _frontBorder;
    [SerializeField] private Transform _backBorder;

    [SerializeField] private BombSpawner _bombSpawner;

    private Coroutine _coroutine;
    private bool _isRunning;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        _isRunning = true;
        Pool.ObjectReleased += _bombSpawner.Spawn;
    }

    private void Start() => _coroutine = StartCoroutine(Spawn());

    protected override void OnDisable()
    {
        _isRunning = false;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        Pool.ObjectReleased -= _bombSpawner.Spawn;

        base.OnDisable();
    }

    private IEnumerator Spawn()
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnDelay);

        while (_isRunning)
        {
            yield return wait;
            Pool.Get().transform.position = GetRandomPosition();
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