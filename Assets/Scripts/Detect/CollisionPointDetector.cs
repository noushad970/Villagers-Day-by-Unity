using UnityEngine;

public class CollisionPointDetector : MonoBehaviour
{
    [Header("Reference Objects")]
    public GameObject referenceObjectA;
    public GameObject referenceObjectB;

    [Header("Target Settings")]
    public string targetTag = "FartilizedSubLand";
    public float checkRadius = 0.2f;

    private int collisionCount = 0;
    private Vector3 positionSum = Vector3.zero;
    [SerializeField] private GameObject[] subLand;
    [SerializeField] private ParticleSystem dustParticle;
    [Header("Harvest Settings")]
    public GameObject treePrefab;
    void OnCollisionEnter(Collision collision)
    {
        // Allow collision only between the two reference objects
        if (
            (gameObject == referenceObjectA && collision.gameObject == referenceObjectB) ||
            (gameObject == referenceObjectB && collision.gameObject == referenceObjectA)
           )
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 hitPoint = contact.point;

            Debug.Log("Collision at: " + hitPoint);

            // Store positions
            positionSum += hitPoint;
            collisionCount++;
            if(dustParticle!=null)
            {
                if (!dustParticle.isPlaying)
                {
                  GameObject particle=   Instantiate(dustParticle.gameObject);
                    particle.transform.position = hitPoint;
                    particle.GetComponent<ParticleSystem>().Play();
                    Destroy(particle, 2f);
                }
            }
            // After 5 collisions
            if (collisionCount == 5)
            {
                Vector3 meanPosition = positionSum / 5f;
                Debug.Log("Mean Position: " + meanPosition);
                for (int i = 0; i < subLand.Length; i++)
                {
                    
                    if(meanPosition.x+2>subLand[i].transform.position.x && meanPosition.x - 2 < subLand[i].transform.position.x && meanPosition.z + 1 > subLand[i].transform.position.z && meanPosition.z - 3 < subLand[i].transform.position.z)
                    {
                        subLand[i].SetActive(true);
                    }
                }
              

                // Reset for next cycle
                collisionCount = 0;
                positionSum = Vector3.zero;
            }
        }
    }

    

}
