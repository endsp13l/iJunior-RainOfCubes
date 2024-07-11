using System;
using UnityEngine;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour,IPoolable
{
    [SerializeField] private T _objectPrefab;
    [SerializeField] private int _poolCapacity = 15;
    [SerializeField] private int _poolMaxSize = 30;

    protected Pool<T> Pool;

    public int Amount => Pool.SpawnedObjectsCount;
    public int ActiveAmount => Pool.ActiveObjectsCount;
    
    public event Action AmountChanged;
    
    protected void OnEnable()
    {
        Pool.ObjectGetted += ChangeView;
        Pool.ObjectReleased += ChangeView;
    }

    private void Awake()
    {
        Pool = new Pool<T>(_objectPrefab, _poolCapacity, _poolMaxSize);
        Pool.Initialize();
    }
    
    private void OnDisable()
    {
        Pool.ObjectGetted -= ChangeView;
        Pool.ObjectReleased -= ChangeView;
    }
    
    private void ChangeView(Transform target) => AmountChanged?.Invoke();
    
    private void ChangeView() => AmountChanged?.Invoke();
}