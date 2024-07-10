using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(ColorSelector))]
[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour, IPoolable
{
    private Renderer _renderer;
    private ColorSelector _colorSelector;
    private Color _basicColor;
    private bool _isColorChanged;

    private Coroutine _coroutine;
    private Random _random = new Random();
    private int _minLifetime = 2;
    private int _maxLifetime = 5;

    public event Action<GameObject> Destroyed;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _colorSelector = GetComponent<ColorSelector>();
        _basicColor = _renderer.material.color;
    }

    private void OnEnable()
    {
        _isColorChanged = false;
        _renderer.material.color = _basicColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isPlatform = other.TryGetComponent<Platform>(out _);

        if (isPlatform && _isColorChanged == false)
        {
            _renderer.material.color = _colorSelector.GetRandomColor();
            _isColorChanged = true;

            _coroutine = StartCoroutine(Destroy());
        }
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(GetRandomLifetime());
        Destroyed?.Invoke(gameObject);

        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private float GetRandomLifetime() => _random.Next(_minLifetime, _maxLifetime);
}