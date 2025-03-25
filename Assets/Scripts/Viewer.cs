using TMPro;
using UnityEngine;

public class View : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _spawnedTextBomb;
    [SerializeField] private TextMeshProUGUI _createdTextBomb;
    [SerializeField] private TextMeshProUGUI _activeTextBomb;
    [SerializeField] private TextMeshProUGUI _spawnedTextCube;
    [SerializeField] private TextMeshProUGUI _createdTextCube;
    [SerializeField] private TextMeshProUGUI _activeTextCube;
    
    [SerializeField] private BombSpawner _bombSpawner;
    [SerializeField] private CubeSpawner _cubeSpawner;

    private void OnEnable()
    {
        _cubeSpawner.InfoChanged += ShowInfo;
        _bombSpawner.InfoChanged += ShowInfo;
    }

    private void OnDisable()
    {
        _cubeSpawner.InfoChanged -= ShowInfo;
        _bombSpawner.InfoChanged -= ShowInfo;
    }

    private void ShowInfo()
    {
        UpdateBombInfo();
        UpdateCubeInfo();
    }

    private void UpdateBombInfo()
    {
        _spawnedTextBomb.text = $"{typeof(Bomb)} Spawned: {_bombSpawner.SpawnedCount}";
        _createdTextBomb.text = $"{typeof(Bomb)} Created: {_bombSpawner.CreatedCount}";
        _activeTextBomb.text = $"{typeof(Bomb)} Active: {_bombSpawner.CountActiveInPool()}";
    }

    private void UpdateCubeInfo()
    {
        _spawnedTextCube.text = $"{typeof(Cube)} Spawned: {_cubeSpawner.SpawnedCount}";
        _createdTextCube.text = $"{typeof(Cube)} Created: {_cubeSpawner.CreatedCount}";
        _activeTextCube.text = $"{typeof(Cube)} Active: {_cubeSpawner.CountActiveInPool()}";
    }
}