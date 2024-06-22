using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(ColorSetter))]
[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    public event Action<GameObject> Destroyed;

    private Coroutine _coroutine;
    private ColorSetter _colorSetter;
    private Renderer _renderer;
    private bool _isColorChanged;

    private Random _random = new Random();
    private int _minLifetime = 2;
    private int _maxLifetime = 5;

    private void Awake()
    {
        _colorSetter = GetComponent<ColorSetter>();
        _renderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isPlatform = other.TryGetComponent<Platform>(out _);

        if (isPlatform && _isColorChanged == false)
        {
            _renderer.material.color = _colorSetter.GetRandomColor();
            _isColorChanged = true;

            _coroutine = StartCoroutine(DestroyCube());
        }
    }

    private IEnumerator DestroyCube()
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