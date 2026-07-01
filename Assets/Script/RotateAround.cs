using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float floatSpeed = 0.25f;
    [SerializeField] private float floatDelta = 0.25f;

    private Vector3 _startposition;
    void Start()
    {
        _startposition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        transform.position = _startposition + floatDelta * Mathf.Sin(Time.time * floatSpeed) * transform.up;
    }
}
