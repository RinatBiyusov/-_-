using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class CubeSpawner : GenericSpawner<Cube>
{
    [SerializeField] private GameObject _mainGround;
    [SerializeField] private float _positionY = 50;
    [SerializeField] private float _repeatRate = 2;

    public event Action<Vector3> Released;
    
    protected override void Start()
    {
        base.Start();
        InvokeRepeating(nameof(GetCube), 0, _repeatRate);
    }

    protected override void ActionOnGet(Cube cube)
    {
        base.ActionOnGet(cube);
        cube.Encountered += Release;
        cube.transform.position = GetRandomSpawnPoint();
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
        
        Released?.Invoke(cube.transform.position);
        
        cube.transform.position = GetRandomSpawnPoint();
        cube.Rigidbody.velocity = Vector3.zero;
        
        Pool.Release(cube);
    }

    private void GetCube() =>
        Pool.Get();
}