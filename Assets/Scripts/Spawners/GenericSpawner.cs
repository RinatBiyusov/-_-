using System;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using UnityEngine.UIElements;

public class GenericSpawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;
    [SerializeField] private TextMeshProUGUI _spawnedText;
    [SerializeField] private TextMeshProUGUI _createdText;
    [SerializeField] private TextMeshProUGUI _activeText;
    
    private readonly int _poolCapacity = 10;
    private readonly int _maxPoolCapacity = 15;
    
    private int _spawnedCount = 0;
    private int _createdCount = 0;
    
    protected ObjectPool<T> Pool;
    
    protected virtual void Awake()
    {
        Pool = new ObjectPool<T>(
            createFunc: () =>
            {
                _createdCount++;
                UpdateUI();
                return Instantiate(_prefab);
            },
            actionOnGet: ActionOnGet,
            actionOnRelease: (T) =>
            {
                UpdateUI();
                T.gameObject.SetActive(false);
            },
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _maxPoolCapacity);
    }

    protected virtual void Start()
    {
        _spawnedText.text = $"{typeof(T)} Spawned: 0";
        _createdText.text = $"{typeof(T)} Created: 0";
        _activeText.text = $"{typeof(T)} Active: 0";
    }
    
    protected virtual void ActionOnGet(T prefab)
    {
        _spawnedCount++;
        prefab.gameObject.SetActive(true);
        UpdateUI();
    }

    private void UpdateUI()
    {
        _spawnedText.text = $"{typeof(T)} Spawned: {_spawnedCount}";
        _createdText.text = $"{typeof(T)} Created: {_createdCount}";
        _activeText.text = $"{typeof(T)} Active: {Pool.CountActive}";
    }
}