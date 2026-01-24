using UnityEngine;

public class TireSpin : MonoBehaviour
{
    [SerializeField] private Transform[] objectsToRotate; // assign 4 objects here
    [SerializeField] private float rotationSpeed = 90f;   // degrees per second

    private bool isRotating = false;
    private void Start()
    {
        StartRotation();
    }
    void Update()
    {
        if (!isRotating) return;

        foreach (Transform obj in objectsToRotate)
        {
            if (obj != null)
            {
                obj.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
            }
        }
    }

    // Call this to start rotation
    public void StartRotation()
    {
        isRotating = true;
    }

    // Call this to stop rotation
    public void StopRotation()
    {
        isRotating = false;
    }
}
