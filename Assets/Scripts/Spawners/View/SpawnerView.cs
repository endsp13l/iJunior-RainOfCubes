using UnityEngine;
using TMPro;

public class SpawnerView<T> : MonoBehaviour where T : MonoBehaviour, IPoolable
{
    [SerializeField] private Spawner<T> _spawner;

    [SerializeField] private TextMeshProUGUI _objectNameText;
    [SerializeField] private TextMeshProUGUI _spawnedObjectsText;
    [SerializeField] private TextMeshProUGUI _activeObjectsText;

    private void OnEnable() => _spawner.AmountChanged += UpdateTexts;
    
    private void Awake() => _objectNameText.text = $"{typeof(T).Name}";

    private void OnDisable() => _spawner.AmountChanged -= UpdateTexts;

    private void UpdateTexts()
    {
        _spawnedObjectsText.text = _spawner.Amount.ToString();
        _activeObjectsText.text = _spawner.ActiveAmount.ToString();
    }
}