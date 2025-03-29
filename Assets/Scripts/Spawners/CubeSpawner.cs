using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

public class CubeSpawner : GenericSpawner<Cube>
{
    [SerializeField] private Platform _mainGround;
    [SerializeField] private float _positionY = 50;
    [SerializeField] private float _repeatRate = 2;
    
    private WaitForSeconds _repeatWait;
    
    public event Action<Vector3> Released;

    private void Start()
    {
        _repeatWait = new WaitForSeconds(_repeatRate);
        
        StartCoroutine(Spawn());
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

        ReleaseItem(cube);
    }

    private IEnumerator Spawn()
    {
        while (enabled)
        {
            GetItem();
            yield return _repeatWait;
        }
    }
}