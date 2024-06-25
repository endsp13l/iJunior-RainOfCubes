using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(Renderer))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private int _minLifetime = 2;
    [SerializeField] private int _maxLifetime = 5;
    [SerializeField] private float _explosionForce = 20f;
    [SerializeField] private float _explosionRadius = 1f;
    [SerializeField] private LayerMask _cubesLayerMask;

    private Random _random = new Random();
    private Material _material;
    private Coroutine _destroyCoroutine;
    private Coroutine _dissappearCoroutine;


    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
    }

    private void OnEnable()
    {
        _destroyCoroutine = StartCoroutine(Activate());
    }

    private IEnumerator Activate()
    {
        int lifetime = GetRandomLifetime();
        _dissappearCoroutine = StartCoroutine(Disappear(lifetime));

        yield return new WaitForSeconds(lifetime);

        if (_destroyCoroutine != null)
            StopCoroutine(_destroyCoroutine);

        Explode();
    }

    private IEnumerator Disappear(float time)
    {
        float alpha = _material.color.a;

        while (alpha > 0)
        {
            alpha = Mathf.MoveTowards(alpha, 0, Time.deltaTime / time);
            _material.color = new Color(_material.color.r, _material.color.g, _material.color.b, alpha);

            yield return null;
        }

        if (_dissappearCoroutine != null)
            StopCoroutine(_dissappearCoroutine);
    }

    private void Explode()
    {
        foreach (Rigidbody cube in GetCubes())
            cube.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);

        Destroy(gameObject);
    }

    private List<Rigidbody> GetCubes()
    {
        List<Rigidbody> cubes = new List<Rigidbody>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius, _cubesLayerMask);

        foreach (Collider collider in colliders)
        {
            if (collider.attachedRigidbody)
                cubes.Add(collider.attachedRigidbody);
        }

        return cubes;
    }

    private int GetRandomLifetime() => _random.Next(_minLifetime, _maxLifetime);
}