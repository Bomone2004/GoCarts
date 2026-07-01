using UnityEngine;

public class SpeedometerManager : MonoBehaviour
{
    [SerializeField] private RectTransform pinRT;
    [SerializeField] private float maxAngle = 230;


    private void OnEnable()
    {
        CartController.CarSpeed += UpdateSpeed;
    }
    private void OnDisable()
    {
        CartController.CarSpeed  -= UpdateSpeed;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void UpdateSpeed(float speedRatio)
    {
        pinRT.rotation = Quaternion.Euler(0, 0, -Mathf.Lerp(0, 230, speedRatio));
    }

}