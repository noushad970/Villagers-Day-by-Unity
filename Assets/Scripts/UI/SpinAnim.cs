using UnityEngine;

public class SpinAnim : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 100f;

    [Header("Squeeze (Scale Animation)")]
    [SerializeField] private float squeezeSpeed = 2f;   // how fast it pulses
    [SerializeField] private float squeezeAmount = 0.2f; // how much it squeezes

    private RectTransform rectTransform;
    private Vector3 originalScale;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    void Update()
    {
        // 🔄 Continuous rotation (Z axis)
        rectTransform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        // 🔁 Squeeze effect using sine wave
        float squeeze = Mathf.Sin(Time.time * squeezeSpeed) * squeezeAmount;

        // Scale X and Y opposite for "squeeze" feel
        float scaleX = originalScale.x + squeeze;
        float scaleY = originalScale.y - squeeze;

        rectTransform.localScale = new Vector3(scaleX, scaleY, originalScale.z);
    }
}