using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class CuttingTreeCollisionDetector : MonoBehaviour
{
    public Animator animator;

    private bool hasHitThisChop = false;
    public int hitCount = 0;
    [SerializeField] private ParticleSystem hitParticle;
    public ParticleSystem treeDestroyParticla;
    public GameObject treeWoodPrefab;
    Rigidbody rb;
    public float animDetectionTime = 1.5f; // Time window to detect hits during chopping animation  
    public RaycastDetector detector;
    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            animator = player.GetComponent<Animator>();
            detector = player.GetComponent<RaycastDetector>();
        }
        rb = GetComponent<Rigidbody>();
        // Find AxeEffect in hierarchy
        GameObject effectObj = GameObject.Find("AxeEffect");
        if (effectObj != null)
        {
            hitParticle = effectObj.GetComponent<ParticleSystem>();
        }

    }
    private void Update()
    {
        // If animation finished, reset hit flag
        if (!IsChopping())
        {
            hasHitThisChop = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsChopping()) return;
        if (hasHitThisChop) return;

        if (collision.gameObject.CompareTag("TreeHitPoint"))
        {
            Debug.Log("Tree Hit During Chopping!");
            ParticleSystem p= Instantiate(hitParticle, collision.gameObject.transform,collision.transform);
            p.Play();
            hitCount++;
            hasHitThisChop = true;


            NoticeUI.Instance.ShowNotice($"Tree Hit! ({hitCount}/3)");
            if (hitCount == 3)
            {
                RotateTree(this.gameObject);
                rb.isKinematic = false;
                //spawn tree wood prefab here
                StartCoroutine(destroyTree());

            }
            Destroy(p.gameObject, 4f);
            //
        }
    }
    IEnumerator destroyTree()
    {
        yield return new WaitForSeconds(5f);
        treeWoodPrefab.SetActive(true);
        GameObject woodStack=treeWoodPrefab;

        Instantiate(woodStack, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject); 
        detector.RemoveTree(this.gameObject);
        treeDestroyParticla.Play();
        StartCoroutine(destroyWoodStack(woodStack));
    }
    IEnumerator destroyWoodStack(GameObject gm)
    {
               yield return new WaitForSeconds(40f);
       Destroy(gm);

    }
    private void RotateTree(GameObject treeObject)
    {
        Vector3 newRotation = treeObject.transform.eulerAngles;
        newRotation.x = -1f;

        treeObject.transform.eulerAngles = newRotation;

        Debug.Log("Tree rotated on X axis!");
    }


    bool IsChopping()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName("Chopping") && stateInfo.normalizedTime < animDetectionTime;
    }
}
