using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private int _poolCapacity = 15;
    [SerializeField] private int _poolMaxSize = 30;
    [SerializeField] private float _spawnDelay = 0.25f;

    private Pool<Bomb> _bombPool;
    private Coroutine _coroutine;
    private bool _isRunning;

    private void Awake()
    {
        _bombPool = new Pool<Bomb>(_bombPrefab, _poolCapacity, _poolMaxSize);
        _bombPool.Initialize();
    }

    public void Spawn(Transform target)
    {
        GameObject bomb = _bombPool.Get();
        bomb.transform.position = target.position;
    }
}