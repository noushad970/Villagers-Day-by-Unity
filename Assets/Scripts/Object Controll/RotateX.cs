using UnityEngine;

public class RotateX : MonoBehaviour
{
    public float rotationSpeed = 90f; // degrees per second
    public string rotateAxis;
    void Update()
    {
        if(rotateAxis == "X")
        {
            transform.localRotation *= Quaternion.Euler(rotationSpeed * Time.deltaTime, 0, 0);
        }
        else if(rotateAxis == "Y")
        {
            transform.localRotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
        }
        else if(rotateAxis == "Z")
        {
            transform.localRotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);
        }
    }
}
