using System;
using UnityEngine;
using UnityEngine.Pool;

public class Pool<T> where T : MonoBehaviour, IPoolable
{
    private T _objectPrefab;
    private int _poolCapacity;
    private int _poolMaxSize;

    private ObjectPool<GameObject> _objectPool;
    private int _spawnedObjectsCount;

    public int SpawnedObjectsCount => _spawnedObjectsCount;
    public int ActiveObjectsCount => _objectPool.CountActive;

    public event Action<Transform> ObjectReleased;
    public event Action ObjectGetted;

    public Pool(T objectPrefab, int poolCapacity, int poolMaxSize)
    {
        _objectPrefab = objectPrefab;
        _poolCapacity = poolCapacity;
        _poolMaxSize = poolMaxSize;
    }

    public void Initialize()
    {
        _objectPool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => ActionOnRelease(obj),
            actionOnDestroy: (obj) => ActionOnDestroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    public GameObject Get()
    {
        ObjectGetted?.Invoke();

        return _objectPool.Get();
    }

    private GameObject CreateObject()
    {
        GameObject obj = GameObject.Instantiate(_objectPrefab.gameObject);

        GetCurrentTypeComponent(obj).Destroyed += _objectPool.Release;
        obj.SetActive(false);

        return obj;
    }

    private void ActionOnGet(GameObject obj)
    {
        StopObjectMovement(obj);

        obj.SetActive(true);
        _spawnedObjectsCount++;
    }

    private void ActionOnRelease(GameObject obj)
    {
        ObjectReleased?.Invoke(obj.transform);

        obj.SetActive(false);
    }

    private void ActionOnDestroy(GameObject gameObject)
    {
        GetCurrentTypeComponent(gameObject).Destroyed -= _objectPool.Release;
        GameObject.Destroy(gameObject);
    }

    private T GetCurrentTypeComponent(GameObject obj)
    {
        obj.TryGetComponent(out T component);
        return component;
    }

    private void StopObjectMovement(GameObject obj)
    {
        obj.TryGetComponent(out Rigidbody rigidbody);
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        obj.transform.rotation = Quaternion.identity;
    }
}