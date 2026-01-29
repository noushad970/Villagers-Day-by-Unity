using System.Collections;
using UnityEngine;

public class AnimalLifeCycle : MonoBehaviour
{
    [Header("Food & Water Points")]
    [SerializeField] private FoodAndWaterPoint[] eatAndDrinkPoints;   // ← changed to array

    private Animator anim;
    [SerializeField] private string animalName;
    public enum animalState
    {
        idleState,
        hungerState,
        feedState,
        drinkAndEatState,
        giveEggState
    }

    public animalState currentState;

    // ------------------ MOVEMENT ------------------
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float stopRadius = 2f;
    public float reachDistance = 0.3f;
    public float rotationSpeed = 5f;

    // ------------------ RANDOM WALK ------------------
    [Header("Random Walk Points")]
    public GameObject[] randomWalkPoints;

    private Transform currentTarget;

    private void Start()
    {
        anim = GetComponent<Animator>();
        currentState = animalState.idleState;
        PickRandomPoint();

        // ✅ Start lifecycle ONLY ONCE
        StartCoroutine(LifeCycle());
    }
    private void initializeAnimal()
    {
        if(animalName=="Chicken")
        {

        }
        else if(animalName=="Cow")
        {
        }
        else if(animalName=="Sheep")
        {
        }else if(animalName=="Pig")
        {
        }
        else if(animalName=="Goat")
        {
        }
    }
    private void Update()
    {
        switch (currentState)
        {
            case animalState.idleState:
            case animalState.drinkAndEatState:
                anim.Play("Walk");
                RandomWalk();
                break;

            case animalState.hungerState:
                CheckFood();
                break;

            case animalState.feedState:
            case animalState.giveEggState:
                anim.Play("Idle");
                break;
        }
    }

    // ================== LIFE CYCLE ==================
    IEnumerator LifeCycle()
    {
        while (true)
        {
            switch (currentState)
            {
                case animalState.idleState:
                    yield return new WaitForSeconds(Random.Range(20, 40));
                    currentState = animalState.hungerState;
                    Debug.Log("Animal is now hungry");
                    break;

                case animalState.feedState:
                    anim.Play("Idle");
                    yield return new WaitForSeconds(Random.Range(5, 10));
                    currentState = animalState.drinkAndEatState;
                    break;

                case animalState.drinkAndEatState:
                    yield return new WaitForSeconds(Random.Range(35, 60));
                    currentState = animalState.giveEggState;
                    Debug.Log("Animal is ready to give egg");
                    break;

                case animalState.hungerState:
                case animalState.giveEggState:
                    yield return null;
                    break;
            }
            yield return null;
        }
    }

    // ================== FOOD LOGIC ==================
    void CheckFood()
    {
        // Look for the first available food point
        foreach (var point in eatAndDrinkPoints)
        {
            if (point != null && point.checkFood())
            {
                MoveToFood(point);
                return;   // found and moving → exit loop
            }
        }

        Debug.Log("No food available");
    }

    void MoveToFood(FoodAndWaterPoint point)
    {
        float distance = Vector3.Distance(transform.position, point.transform.position);

        if (distance > stopRadius)
        {
            Vector3 dir = (point.transform.position - transform.position).normalized;
            dir.y = 0f;
            transform.position += dir * moveSpeed * Time.deltaTime;
            RotateTowards(dir);
            anim.Play("Walk");
        }
        else
        {
            Debug.Log("Reached food point");
            point.foodObject.SetActive(false);
            StartCoroutine(FeedingState());
        }
    }

    IEnumerator FeedingState()
    {
        currentState = animalState.feedState;
        anim.Play("Idle");
        yield return new WaitForSeconds(Random.Range(5, 10));
        currentState = animalState.drinkAndEatState;
    }

    // ================== RANDOM WALK ==================
    void RandomWalk()
    {
        if (currentTarget == null || randomWalkPoints.Length == 0)
            return;

        float distance = Vector3.Distance(transform.position, currentTarget.position);

        if (distance <= reachDistance)
        {
            PickRandomPoint();
        }

        Vector3 dir = (currentTarget.position - transform.position).normalized;
        dir.y = 0f;
        transform.position += dir * moveSpeed * Time.deltaTime;
        RotateTowards(dir);
    }

    void PickRandomPoint()
    {
        int index = Random.Range(0, randomWalkPoints.Length);
        currentTarget = randomWalkPoints[index].transform;
    }

    // ================== ROTATION ==================
    void RotateTowards(Vector3 direction)
    {
        if (direction.sqrMagnitude == 0) return;

        Quaternion targetRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotationSpeed * Time.deltaTime
        );
    }
}