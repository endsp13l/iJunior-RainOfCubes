using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(ColorSetter))]
[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{

    private Renderer _renderer;
    private ColorSetter _colorSetter;
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
        _colorSetter = GetComponent<ColorSetter>();
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
            _renderer.material.color = _colorSetter.GetRandomColor();
            _isColorChanged = true;

            _coroutine = StartCoroutine(Destroy());
        }
    }

    private IEnumerator Destroy()
    {
        WaitForSeconds wait = new WaitForSeconds(GetRandomLifetime());

        yield return wait;
        Destroyed?.Invoke(gameObject);

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private float GetRandomLifetime() => _random.Next(_minLifetime, _maxLifetime);
}