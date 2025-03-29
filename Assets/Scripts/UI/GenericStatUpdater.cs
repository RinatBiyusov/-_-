using System;
using UnityEngine;
using TMPro;

public class GenericStatUpdater<T> : MonoBehaviour where T : Item
{
    [SerializeField] private TextMeshProUGUI _spawnedTextObject;
    [SerializeField] private TextMeshProUGUI _createdTextObject;
    [SerializeField] private TextMeshProUGUI _activeTextObject;

    [SerializeField] protected GenericSpawner<T> _spawner;

    private void OnEnable()
    {
        _spawner.InfoChanged += ShowInfo;
    }

    private void Start()
    {
        ShowInfo();
    }

    private void OnDisable()
    {
        _spawner.InfoChanged -= ShowInfo;
    }

    private void ShowInfo()
    {
        _spawnedTextObject.text = $"{typeof(T)} Spawned: {_spawner.SpawnedCount}";
        _createdTextObject.text = $"{typeof(T)} Created: {_spawner.CreatedCount}";
        _activeTextObject.text = $"{typeof(T)} Active: {_spawner.CountActiveInPool()}";
    }
}