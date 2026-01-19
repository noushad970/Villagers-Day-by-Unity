using UnityEngine;

public class CollisionPointDetector : MonoBehaviour
{
    public int requiredHits = 5;

    private int hitCount = 0;

    public GameObject land;
    public GameObject subLand;
    [SerializeField] private ParticleSystem dustParticle,SublandParticle;

    private LandDataComponent landData; // Reference to LandDataComponent

    void Start()
    {
        // Get the LandDataComponent attached to this parent
        landData = GetComponent<LandDataComponent>();

        foreach (Transform child in transform)
        {
            if (child.CompareTag("FartilizedLand"))
                land = child.gameObject;

            if (child.CompareTag("FartilizedSubLand"))
                subLand = child.gameObject;
        }
        
    }
    private void Update()
    {
        if (landData.isFertilized)
        {
            if (land != null) land.SetActive(false);
            if (subLand != null) subLand.SetActive(true);
        }
        else
        {
            if (land != null) land.SetActive(true);
            if (subLand != null) subLand.SetActive(false);
        }
    }

    // 👇 This will be called from child colliders
    [System.Obsolete]
    public void RegisterHit(GameObject hitter)
    {
        if (!hitter.CompareTag("PlowHitPoint"))
            return;

        hitCount++;
        Debug.Log("Plow hit count: " + hitCount);

        // Play dust particle
        if (dustParticle != null)
        {
            GameObject dp = Instantiate(dustParticle.gameObject, hitter.transform.position, Quaternion.identity);
            dp.GetComponent<ParticleSystem>().Play();
            Destroy(dp, 4f);

        }

        if (hitCount >= requiredHits)
        {
            if (land != null) land.SetActive(false);
            if (subLand != null) subLand.SetActive(true);
            SublandParticle.Play();
            Debug.Log("Land switched to SubLand");

            // ✅ Update the LandDataComponent state
            if (landData != null)
            {
                landData.isFertilized = true;

                // Save the farm immediately
                FarmController farmController = FindObjectOfType<FarmController>();
                if (farmController != null)
                {
                    farmController.SaveFarm();
                    Debug.Log("Farm saved after sub-land activation.");
                }
            }
        }
    }
}
