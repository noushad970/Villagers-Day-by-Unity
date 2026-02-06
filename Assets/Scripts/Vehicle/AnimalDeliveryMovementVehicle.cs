using System.Collections;
using UnityEngine;

public class AnimalDeliveryMovementVehicle : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] private GameObject[] points;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float reachDistance = 0.1f;

    [Header("Logic")]
    public bool canMove = false;

    private int currentPointIndex = 0;
    private bool isVisiting = false;
    [SerializeField] public GameObject[] animals;
    public static bool vehicleIsBusy = false;

    private void Start()
    {
       canMove = false;

    }
    void Update()
    {
        if (points == null || points.Length == 0) return;
        if (canMove)
        {
            StartCoroutine(canMoveWait());
        }
        // Start a new visit
        if (canMove && !isVisiting)
        {
            StartNewVisit();
        }

        // Continue visit even if canMove becomes false
        if (isVisiting)
        {
            string animalName = "";
            for (int i = 0; i < animals.Length; i++)
            {
                if (animals[i].activeSelf)
                {
                    animalName = animals[i].tag.ToString();
                }
            }
            MoveToPoint(animalName);
        }
        Debug.Log("Can move Vehicle: " + canMove);
    }

    void StartNewVisit()
    {
        currentPointIndex = 0;
        isVisiting = true;

        //////
        ///

    }
    public void deliverAnimal(string animalName,bool move)
    {
        vehicleIsBusy = true;
        canMove = move;
        for(int i=0;i<animals.Length;i++)
        {
            if(animals[i].tag.ToString()==animalName)
            {
                animals[i].SetActive(true);
            }
            else
            {
                animals[i].SetActive(false);
            }
        }

    }
    IEnumerator canMoveWait()
    {
        yield return new WaitForSeconds(2f);
        if (canMove)
            canMove = false;
    }
    void MoveToPoint(string animalName)
    {
        Transform target = points[currentPointIndex].transform;
        if (target.CompareTag("SpawnAnimalPoint"))
        {
            ///
            if(animalName!="")
            {
                Debug.Log("Spawned Animal: " + animalName);
                Debug.Log("Total Cow: " + PlayerSaveManager.Instance.GetItemCount(animalName));

                AnimalSpawner.instance.SpawnAnimal(animalName);
                //set animal inactive after spawn
                PlayerSaveManager.Instance.AddItem(animalName, 1);
                for (int i = 0; i < animals.Length; i++)
                {
                    if (animals[i].tag.ToString() == animalName)
                    {
                        animals[i].SetActive(false);
                    }
                }
            }

        }
        // Move
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        // Rotate toward target
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Reach check
        if (Vector3.Distance(transform.position, target.position) <= reachDistance)
        {
            currentPointIndex++;

            // Finished all waypoints
            if (currentPointIndex >= points.Length)
            {
                isVisiting = false; // stop only after finishing visit
                vehicleIsBusy = false;
            }
        }
    }
}
