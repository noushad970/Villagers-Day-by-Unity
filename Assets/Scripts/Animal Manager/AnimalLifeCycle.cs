using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class AnimalLifeCycle : MonoBehaviour
{
    [Header("Food & Water Points")]
    [SerializeField] private FoodAndWaterPoint[] eatAndDrinkPoints;   // ← changed to array

    private Animator anim;
    [SerializeField] private string animalName;
    public Transform collectableItem;
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
     float amplitude = 0.3f;   // How high it moves
     float frequency = 5f;     // Speed of movement
    public Vector3 yUp =new Vector3(0, 1.5f,0);

    private Vector3 startPos;

    

    
    private void Start()
    {
        if(this.gameObject.CompareTag("Goat1")||this.gameObject.CompareTag("Goat2"))
        startPos = this.gameObject.transform.position+yUp;
        anim = GetComponent<Animator>();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("CollectableAnimalItem"))
            {
                collectableItem = child;
            }
        }
        collectableItem.gameObject.SetActive(false);
        animalName = gameObject.tag.ToString();
        initializeAnimal();
        currentState = animalState.idleState;
        PickRandomPoint();
        // ✅ Start lifecycle ONLY ONCE
        StartCoroutine(LifeCycle());
    }
    private void initializeAnimal()
    {
        if (animalName == "Chicken")
        {
            GameObject[] foodPoint = GameObject.FindGameObjectsWithTag("EatAndDrinkPointChicken");
            for (int i = 0; i < foodPoint.Length; i++)
            {
                eatAndDrinkPoints[i] = foodPoint[i].GetComponent<FoodAndWaterPoint>();
            }
            GameObject[] wayPoints = GameObject.FindGameObjectsWithTag("WayPointChicken");
            for (int i = 0; i < wayPoints.Length; i++)
            {
                randomWalkPoints[i] = wayPoints[i];
            }
        }
        else if (animalName == "Cow")
        {
            GameObject[] foodPoint = GameObject.FindGameObjectsWithTag("EatAndDrinkPointCow");
            for (int i = 0; i < foodPoint.Length; i++)
            {
                eatAndDrinkPoints[i] = foodPoint[i].GetComponent<FoodAndWaterPoint>();
            }
            GameObject[] wayPoints = GameObject.FindGameObjectsWithTag("WayPointCow");
            for (int i = 0; i < wayPoints.Length; i++)
            {
                randomWalkPoints[i] = wayPoints[i];
            }
        }
        else if (animalName == "Sheep")
        {
            GameObject[] foodPoint = GameObject.FindGameObjectsWithTag("EatAndDrinkPointSheep");
            for (int i = 0; i < foodPoint.Length; i++)
            {
                eatAndDrinkPoints[i] = foodPoint[i].GetComponent<FoodAndWaterPoint>();
            }
            GameObject[] wayPoints = GameObject.FindGameObjectsWithTag("WayPointSheep");
            for (int i = 0; i < wayPoints.Length; i++)
            {
                randomWalkPoints[i] = wayPoints[i];
            }
        }
        else if (animalName == "Pig")
        {
        }
        else if (gameObject.CompareTag("Goat2"))
        {
            GameObject[] foodPoint = GameObject.FindGameObjectsWithTag("EatAndDrinkPointGoat2");
            for (int i = 0; i < foodPoint.Length; i++)
            {
                eatAndDrinkPoints[i] = foodPoint[i].GetComponent<FoodAndWaterPoint>();
            }
            GameObject[] wayPoints = GameObject.FindGameObjectsWithTag("WayPointGoat2");
            for (int i = 0; i < wayPoints.Length; i++)
            {
                randomWalkPoints[i] = wayPoints[i];
            }
        }
        else if (gameObject.CompareTag("Goat1"))
        {
            GameObject[] foodPoint = GameObject.FindGameObjectsWithTag("EatAndDrinkPointGoat1");
            for (int i = 0; i < foodPoint.Length; i++)
            {

                eatAndDrinkPoints[i] = foodPoint[i].GetComponent<FoodAndWaterPoint>();
            }
            GameObject[] wayPoints = GameObject.FindGameObjectsWithTag("WayPointGoat1");
            for (int i = 0; i < wayPoints.Length; i++)
            {
                randomWalkPoints[i] = wayPoints[i];
            }
        }
        else if (animalName == "Duck")
        {
            GameObject[] foodPoint = GameObject.FindGameObjectsWithTag("EatAndDrinkPointDuck");
            for (int i = 0; i < foodPoint.Length; i++)
            {
                eatAndDrinkPoints[i] = foodPoint[i].GetComponent<FoodAndWaterPoint>();
            }
            GameObject[] wayPoints = GameObject.FindGameObjectsWithTag("WayPointDuck");
            for (int i = 0; i < wayPoints.Length; i++)
            {
                randomWalkPoints[i] = wayPoints[i];
            }
        }
        else if (animalName == "Sheep2")
        {
            GameObject[] foodPoint = GameObject.FindGameObjectsWithTag("EatAndDrinkPointSheep2");
            for (int i = 0; i < foodPoint.Length; i++)
            {
                eatAndDrinkPoints[i] = foodPoint[i].GetComponent<FoodAndWaterPoint>();
            }
            GameObject[] wayPoints = GameObject.FindGameObjectsWithTag("WayPointSheep2");
            for (int i = 0; i < wayPoints.Length; i++)
            {
                randomWalkPoints[i] = wayPoints[i];
            }
        }
    }
    private void Update()
    {

        if (this.gameObject.CompareTag("Goat1") || this.gameObject.CompareTag("Goat2"))
            startPos = this.gameObject.transform.position + yUp;

        if (this.gameObject.CompareTag("Goat1") || this.gameObject.CompareTag("Goat2"))
        {
            float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
            collectableItem.transform.position = startPos + new Vector3(0f, yOffset, 0f);
        }
        switch (currentState)
        {
            case animalState.idleState:
                anim.Play("Walk");
                RandomWalk();
                break;
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
                collectableItem.gameObject.SetActive(true);
                break;
        }
        if (currentState == animalState.giveEggState)
        {

            collectableItem.gameObject.SetActive(true);
        }
        else
        {
            collectableItem.gameObject.SetActive(false);
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

                    break;

                case animalState.feedState:
                    anim.Play("Idle");
                    yield return new WaitForSeconds(Random.Range(5, 10));
                    currentState = animalState.drinkAndEatState;
                    break;

                case animalState.drinkAndEatState:
                    yield return new WaitForSeconds(Random.Range(35, 60));
                    currentState = animalState.giveEggState;
                    break;

                case animalState.hungerState:
                case animalState.giveEggState:
                    yield return null;
                    break;
            }
            yield return null;
        }
    }
    public void CollectItem()
    {
        if (currentState == animalState.giveEggState)
        {
            collectableItem.gameObject.SetActive(false);
            NoticeUI.Instance.ShowNotice("Collected "+collectableItem.gameObject.name.ToString()+" from " + animalName); 
            PlayerSaveManager.Instance.AddItem(collectableItem.gameObject.name.ToString(), 1);
            currentState = animalState.idleState;
        }
    }
    public bool isReadyToCollect()
    {
        if (currentState == animalState.giveEggState)
        {
            return true;
        }
        else
        {
            return false;
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
            anim.Play("Idle");
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
