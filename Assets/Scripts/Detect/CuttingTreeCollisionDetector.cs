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
    public TreeSaveManager treeSaveManager;
    GameObject Tree;
    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject Tree = GameObject.FindGameObjectWithTag("GameManager");


        if (player != null)
        {
            animator = player.GetComponent<Animator>();
            detector = player.GetComponent<RaycastDetector>();
        }
        StartCoroutine(FindGameManagerRoutine());

        rb = GetComponent<Rigidbody>();
        // Find AxeEffect in hierarchy
        

    }
    IEnumerator FindGameManagerRoutine()
    {
        while (Tree == null)
        {
            Tree = GameObject.FindGameObjectWithTag("GameManager");

            if (Tree != null)
            {
                Debug.Log("GameManager Found: " + Tree.name);
                OnGameManagerFound();
                yield break;
            }

            yield return new WaitForSeconds(0.5f); // check every 0.5 seconds
        }
    }
    void OnGameManagerFound()
    {
        // Your logic after finding GameMan
        treeSaveManager = Tree.GetComponent<TreeSaveManager>();
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
            hitParticle.Play();
            hitCount++;
            hasHitThisChop = true;
            AudioManager.Instance.playAxeHitSound();

            NoticeUI.Instance.ShowNotice($"Tree Hit! ({hitCount}/3)");
            if (hitCount == 3)
            {
                RotateTree(this.gameObject);
                rb.isKinematic = false;

                treeSaveManager.ChangeTreeState(this.gameObject, "Cutted");
                //spawn tree wood prefab here
                StartCoroutine(destroyTree());

            }
            //
        }
        else
        {
            AudioManager.Instance.playNoHittingSound();
        }
    }
    IEnumerator destroyTree()
    {
        AudioManager.Instance.playDestroyTreeSound();
        yield return new WaitForSeconds(5f);
        treeWoodPrefab.SetActive(true);
        GameObject woodStack=treeWoodPrefab;

        Instantiate(woodStack, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject); 
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
