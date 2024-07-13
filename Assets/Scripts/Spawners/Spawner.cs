using System;
using UnityEngine;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour, IPoolable
{
    [SerializeField] private T _objectPrefab;
    [SerializeField] private int _poolCapacity = 15;
    [SerializeField] private int _poolMaxSize = 30;

    protected Pool<T> Pool;

    public int Amount => Pool.SpawnedObjectsCount;
    public int ActiveAmount => Pool.ActiveObjectsCount;

    public event Action AmountChanged;

    protected virtual void OnEnable()
    {
        Pool.ObjectGetted += UpdateView;
        Pool.ObjectReleased += UpdateView;
    }

    private void Awake()
    {
        Pool = new Pool<T>(_objectPrefab, _poolCapacity, _poolMaxSize);
        Pool.Initialize();
    }

    protected virtual void OnDisable()
    {
        Pool.ObjectGetted -= UpdateView;
        Pool.ObjectReleased -= UpdateView;
    }

    private void UpdateView(Transform target) => AmountChanged?.Invoke();

    private void UpdateView() => AmountChanged?.Invoke();
}