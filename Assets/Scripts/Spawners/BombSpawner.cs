using UnityEngine;

public class BombSpawner : GenericSpawner<Bomb>
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    private void OnEnable()
    {
        _cubeSpawner.Released += CreateBomb;
    }

    private void OnDisable()
    {
        _cubeSpawner.Released -= CreateBomb;
    }

    private void CreateBomb(Vector3 spawnPoint)
    {
        Bomb bomb = GetPool();
        bomb.transform.position = spawnPoint;
        bomb.Exploded += Release;
    }

    private void Release(Bomb bomb)
    {
        bomb.Exploded -= Release;
        ReleasePool(bomb);
    }
}