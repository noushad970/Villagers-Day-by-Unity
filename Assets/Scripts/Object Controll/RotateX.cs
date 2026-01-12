using UnityEngine;

public class RotateX : MonoBehaviour
{
    public float rotationSpeed = 90f; // degrees per second

    void Update()
    {
        transform.localRotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
    }
}
