using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ColorChanger), typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private ColorChanger _colorChanger;
    private WaitForSeconds _lifeTime;
    private readonly int _minLifeTime = 2;
    private readonly int _maxLifeTime = 5;
    private MeshRenderer _meshRenderer;
    private bool _isEncountered;
    private Color _color;
    private Rigidbody _rigidbody;

    public event Action<Cube> Encountered;
    
    public Rigidbody Rigidbody => _rigidbody;
    
    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _color = _meshRenderer.material.color;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _lifeTime = new(UnityEngine.Random.Range(_minLifeTime, _maxLifeTime + 1));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isEncountered)
            return;

        if (collision.gameObject.TryGetComponent(out Platform platform) == false)
            return;
        
        _isEncountered = true;
        StartCoroutine(ReleaseCube());
    }

    private IEnumerator ReleaseCube()
    {
        _colorChanger.ChangeColor(_meshRenderer);

        yield return _lifeTime;
        
        Encountered?.Invoke(this);
        
        _meshRenderer.material.color = _color;
        _isEncountered = false;
    }
}