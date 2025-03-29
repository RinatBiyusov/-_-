using System;
using UnityEngine;
using UnityEngine.Pool;

public class GenericSpawner<T> : MonoBehaviour where T : Item
{
    [SerializeField] private T _prefab;
    
    private readonly int _poolCapacity = 10;
    private readonly int _maxPoolCapacity = 15;

    private ObjectPool<T> _pool;

    public int SpawnedCount { get;  private set; }
    public int CreatedCount { get;   private set; }

    public event Action InfoChanged;
    
    private void Awake()
    {
        _pool = new ObjectPool<T>(
            createFunc: () =>
            {
                CreatedCount++;
                InfoChanged?.Invoke();
                return Instantiate(_prefab);
            },
            actionOnGet: ActionOnGet,
            actionOnRelease: (T) => T.gameObject.SetActive(false),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _maxPoolCapacity);
    }
    
    protected virtual void ActionOnGet(T prefab)
    {
        SpawnedCount++;
        InfoChanged?.Invoke();
        prefab.gameObject.SetActive(true);
    }

    protected void ReleaseItem(T prefab) =>
        _pool.Release(prefab);

    protected T GetItem() =>
        _pool.Get();

    public int CountActiveInPool() =>
        _pool.CountActive;

}