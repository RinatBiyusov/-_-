using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private float _exploisionRadius;
    [SerializeField] private float _explosionForce;

    private readonly int _minLifeTime = 2;
    private readonly int _maxLifeTime = 5;
    private readonly int _minValueAplha = 0;

    private float _lifeTime;
    private MeshRenderer _meshRenderer;
    private Material _material;
    private Color _baseColor;
    private Coroutine _coroutine;

    public event Action<Bomb> Exploded;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _material = _meshRenderer.material;
        _baseColor = _material.color;
    }

    private void OnEnable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        
        _material.color = _baseColor;
        _lifeTime = UnityEngine.Random.Range(_minLifeTime, _maxLifeTime + 1);
        StartCooldown();
    }

    private void StartCooldown() =>
        _coroutine = StartCoroutine(ExplodeAfterTime());

    private void Explode()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, _exploisionRadius);

        foreach (Collider target in hit)
        {
            if (target.TryGetComponent(out Rigidbody component))
            {
                float attenuation =
                    Mathf.Clamp01(1f - (transform.position - target.transform.position).magnitude / _exploisionRadius);
                Vector3 direction = (target.transform.position - transform.position).normalized;

                if (attenuation > 0)
                    component.AddForce(direction * (_explosionForce * attenuation), ForceMode.Impulse);
            }
        }
    }

    private IEnumerator ExplodeAfterTime()
    {
        float elapsedTime = 0;
        Color initialColor = _material.color;

        while (_lifeTime > elapsedTime)
        {
            float newAlpha = Mathf.Lerp(initialColor.a, _minValueAplha, elapsedTime / _lifeTime);
            elapsedTime += Time.deltaTime;
            _material.color = new Color(initialColor.r, initialColor.g, initialColor.b, newAlpha);

            yield return null;
        }

        Explode();

        Exploded?.Invoke(this);
    }
}