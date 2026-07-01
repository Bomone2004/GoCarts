using UnityEngine;

public class DetectedCar : MonoBehaviour
{
    private Collider _collider;
    private MeshRenderer[] _meshRenderer;
    private float _appearDelay = 5;

    [SerializeField] ParticleSystem _particleSystem;

    void Start()
    {
        _collider = GetComponent<Collider>();
        _meshRenderer = GetComponentsInChildren<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            _collider.enabled = false;
            foreach (var mr in _meshRenderer)
            {
                mr.enabled = false;
            }
            

            _particleSystem.gameObject.SetActive(true);
            _particleSystem.Play();

            Invoke(nameof(ReenableCoin), _appearDelay);
        }
    }

    private void ReenableCoin()
    {
        _collider.enabled = true;
        foreach (var mr in _meshRenderer)
        {
            mr.enabled = true;
        }
        _particleSystem.gameObject.SetActive(false);
    }

}
