using UnityEngine;

public class TreeGrowingUp : MonoBehaviour
{
    public float growthDuration = 60f; // total time to fully grow
    public Vector3 fullScale = Vector3.one; // final size
    public AnimationCurve growthCurve; // optional for smooth effect

    private float timer = 0f;
    private bool isGrowing = false;
    private CuttingTreeCollisionDetector collisionDetector;
    void Start()
    {
        transform.localScale = Vector3.zero;

        collisionDetector = GetComponent<CuttingTreeCollisionDetector>();

        if (collisionDetector != null)
            collisionDetector.enabled = false;

        StartGrowth();
    }

    void Update()
    {
        if (!isGrowing) return;

        timer += Time.deltaTime;

        float t = Mathf.Clamp01(timer / growthDuration);

        // Apply curve if assigned, otherwise linear
        float curveValue = growthCurve != null ? growthCurve.Evaluate(t) : t;

        transform.localScale = Vector3.Lerp(Vector3.zero, fullScale, curveValue);

        if (t >= 1f)
        {
            isGrowing = false;
            OnFullyGrown();
        }
    }

    public void StartGrowth()
    {
        collisionDetector.enabled = false;
        timer = 0f;
        isGrowing = true;
    }

    void OnFullyGrown()
    {
        Debug.Log("Tree fully grown!");
        collisionDetector.enabled = true;
    }
}