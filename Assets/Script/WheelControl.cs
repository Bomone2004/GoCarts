using UnityEngine;

internal class WheelControl : MonoBehaviour
{
    [SerializeField] Transform wheelModel;

    private WheelCollider _whellCollide;

    public WheelCollider WheelCollider => _whellCollide;

    [SerializeField] bool steerable;
    [SerializeField] bool motorized;

    Vector3 position;
    Quaternion rotation;

    public bool Steerable => steerable;
    public bool Motorized => motorized;

    private void Start()
    {
        _whellCollide = GetComponent<WheelCollider>();
    }

    private void Update()
    {
        _whellCollide.GetWorldPose(out position, out rotation);

        if (wheelModel)
        {
            wheelModel.transform.localRotation = Quaternion.Euler(rotation.eulerAngles.x, 0,0);
        }
    }

}