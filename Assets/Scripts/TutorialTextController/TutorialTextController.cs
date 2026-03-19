using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialTextController : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;

    [TextArea(3, 10)]
    public string[] texts;

    public float typingSpeed = 0.03f;

    private int index = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private const string TUTORIAL_KEY = "TutorialDone";
    public Button nextButton;
    void Start()
    {
        startgameButton.gameObject.SetActive(false);
        texts = new string[]
        {
            "Hello! My name is Mr Mango. I am a Happy farmer. I will be with you to explore this beautiful village with you.",

"So first lets talk about our main goal which is Hastle of Hunger...........................",

"You have to collect foods by harvesting or from animal. Food which is very important for your health. To survive in this world and also to fullfil other people's need.",

"Here you can harvest crop, catch fish, collect food from animal also you can buy food from shop. There is a Bazar where you can buy any food.",

"First Buy some crops seed from the store and plant them in the harvesting field. You need to first plow the field where you will harvest them.",

"After planting crops or tree wait for those tree or crops to grow. After complete growing you can collect and or deliver them.",

"You can also collect fish from pond. Goto the near of the pond and select Fishing rod and throw it on the river and wait for fish to catch. when fish bite pull the rod and you will catch a fish. Thats very fun.",

"You can cut down the tree to collect woods. But dont forget to plant another tree when you cut one. As tree is great source of Oxyzen.",

"Goto farm for collecting animal food such as Milk, Egg and wool. You can sell those food or deliver it to the needy peoples.",

"Goto shop to buy anything you want. you can buy fish, meat, crops, vegetables and also animal for your farm.",

"Now you can deliver foods to the needy peoples around you from Deliver food Area. Where you can donate vast amount of food to the needy peoples. And this is our main goal. So lets start the game."
        };
        // ✅ Check if tutorial already completed
        if (PlayerPrefs.GetInt(TUTORIAL_KEY, 0) == 1)
        {
            // Skip tutorial
            SkipTutorial();
            return;
        }

        StartTyping();
        startgameButton.onClick.AddListener(Function2);
        nextButton.onClick.AddListener(OnNextButtonPressed);
    }

    // 👉 Button OnClick
    public void OnNextButtonPressed()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            tutorialText.text = texts[index];
            isTyping = false;
        }
        else
        {
            index++;

            if (index >= texts.Length)
            {
                TutorialFinished();
                return;
            }

            StartTyping();
        }
    }

    void StartTyping()
    {
        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        tutorialText.text = "";

        foreach (char c in texts[index])
        {
            tutorialText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void TutorialFinished()
    {
        Debug.Log("Tutorial Completed!");

        // ✅ Save so it never runs again
        PlayerPrefs.SetInt(TUTORIAL_KEY, 1);
        PlayerPrefs.Save(); // Important for WebGL

        OnTutorialComplete();
    }

    void SkipTutorial()
    {
        Debug.Log("Tutorial already completed. Skipping...");

        // 👉 Call alternate function
        Function2();
    }
    public Button startgameButton;
    void OnTutorialComplete()
    {
        Debug.Log("Run only first time");
        startgameButton.gameObject.SetActive(true);
        // 👉 Your first-time-only logic
        // Example:
        // Enable gameplay, give rewards, etc.
    }
    
    void Function2()
    {
        Debug.Log("Run every time AFTER tutorial is done");
        SceneManager.LoadScene("MainGameScene"); // Example: Load main game scene
        // 👉 Your normal game start logic
        // Example:
        // Load main UI, enable controls, etc.
    }
}