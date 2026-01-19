using UnityEngine;

public class ChildrenCollisionForwarder : MonoBehaviour
{
    private CollisionPointDetector parentSwitcher;
 
    void Start()
    {
        parentSwitcher = GetComponentInParent<CollisionPointDetector>();
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (parentSwitcher != null)
        {
            parentSwitcher.RegisterHit(collision.gameObject);
            Debug.Log("Forwarded collision from " + gameObject.tag + " to parent.");
        }
    }
}
