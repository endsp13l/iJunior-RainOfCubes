using System;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner <T> where T : MonoBehaviour, IPoolable
{
    private T _objectPrefab;
    private int _poolCapacity;
    private int _poolMaxSize;
    
    private int _spawnedObjectsCount;

    public ObjectPool<GameObject> ObjectPool;
    public int SpawnedObjectsCount => _spawnedObjectsCount;
    public int ActiveObjectsCount => ObjectPool.CountActive;

    public event Action ObjectGetted;
    public event Action ObjectReleased;

    public Spawner(T objectPrefab, int poolCapacity, int poolMaxSize)
    {
        _objectPrefab = objectPrefab;
        _poolCapacity = poolCapacity;
        _poolMaxSize = poolMaxSize;
    }

    public void Initialize()
    {
        ObjectPool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => ActionOnRelease(obj),
            actionOnDestroy: (obj) => ActionOnDestroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }


    private GameObject CreateObject()
    {
        GameObject obj = GameObject.Instantiate(_objectPrefab.gameObject);
        
        GetCurrentTypeComponent(obj).Destroyed += ObjectPool.Release;
        obj.SetActive(false);
        
        return obj;
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.SetActive(true);
        _spawnedObjectsCount++;

        ObjectGetted?.Invoke();
    }

    private void ActionOnRelease(GameObject obj)
    {
        obj.SetActive(false);

        ObjectReleased?.Invoke();
    }
    
    private void ActionOnDestroy(GameObject gameObject)
    {
        GetCurrentTypeComponent(gameObject).Destroyed -= ObjectPool.Release;
        GameObject.Destroy(gameObject);
    }
    
    private T GetCurrentTypeComponent(GameObject obj)
    {
        obj.TryGetComponent(out T component);
        return component;
    }
}