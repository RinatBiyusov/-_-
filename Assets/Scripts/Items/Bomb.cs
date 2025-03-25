using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(MeshRenderer))]
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

    public event Action<Bomb> Exploded;

    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _material = _meshRenderer.material;
        Rigidbody = GetComponent<Rigidbody>();
        _baseColor = _material.color;
    }

    private void OnEnable()
    {
        _material.color = _baseColor;
        _lifeTime = UnityEngine.Random.Range(_minLifeTime, _maxLifeTime + 1);
        StartCooldown();
    }

    private void StartCooldown()
    {
        StartCoroutine(ExplodeAfterTime());
    }

    private void Explode()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, _exploisionRadius);

        foreach (var target in hit)
        {
            if (target.TryGetComponent(out Rigidbody rigidbody))
            {
                float attenuation =
                    Mathf.Clamp01(1f - (transform.position - target.transform.position).magnitude / _exploisionRadius);
                Vector3 direction = (target.transform.position - transform.position).normalized;

                if (attenuation > 0)
                    rigidbody.AddForce(direction * (_explosionForce * attenuation), ForceMode.Impulse);
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

        yield return null;
    }
}