using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(ColorSetter))]
[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    private ColorSetter _colorSetter;
    private Renderer _renderer;
    private bool _isColorChanged = false;
    
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
            
            Destroy(gameObject, GetRandomLifetime());
        }
    }

    private float GetRandomLifetime() => _random.Next(_minLifetime, _maxLifetime);
}