using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private GameObject _mainGround;
    [SerializeField] private float _positionY = 50;
    [SerializeField] private float _repeateRate = 2;

    private readonly int _poolCapacity = 10;
    private readonly int _maxPoolCapacity = 15;
    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
        createFunc: () => Instantiate(_prefab),
        actionOnGet: (cube) => ActionOnget(cube),
        actionOnRelease: (cube) => cube.gameObject.SetActive(false),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _maxPoolCapacity);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0, _repeateRate);
    }

    private void ActionOnget(Cube obj)
    {
        obj.Encountered += Release;
        obj.transform.position = GetRandomSpawnPoint();
        obj.gameObject.SetActive(true);
    }

    private Vector3 GetRandomSpawnPoint()
    {
        float positionX = Random.Range(-_mainGround.transform.localScale.x / 2, _mainGround.transform.localScale.x / 2);
        float positionZ = Random.Range(-_mainGround.transform.localScale.z / 2, _mainGround.transform.localScale.z / 2);

        Vector3 spawnPos = new(positionX, _positionY, positionZ);

        return spawnPos;
    }

    private void Release(Cube cube)
    {
        cube.Encountered -= Release;
        cube.gameObject.transform.position = GetRandomSpawnPoint();
        cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _pool.Release(cube);
    }

    private void GetCube()
    {
        _pool.Get();
    }
}