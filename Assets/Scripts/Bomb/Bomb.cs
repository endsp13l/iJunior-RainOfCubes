using System.Collections;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(Renderer))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private int _minLifetime = 2;
    [SerializeField] private int _maxLifetime = 5;

    private Random _random = new Random();
    private Coroutine _destroyCoroutine;
    private Coroutine _dissappearCoroutine;
    private Material _material;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
    }

    private void OnEnable()
    {
        _destroyCoroutine = StartCoroutine(Destroy());
    }


    private IEnumerator Destroy()
    {
        int lifetime = GetRandomLifetime();
        _dissappearCoroutine = StartCoroutine(Disappear(lifetime));

        yield return new WaitForSeconds(lifetime);

        if (_destroyCoroutine != null)
            StopCoroutine(_destroyCoroutine);

        Destroy(gameObject);
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

    private int GetRandomLifetime() => _random.Next(_minLifetime, _maxLifetime);
}