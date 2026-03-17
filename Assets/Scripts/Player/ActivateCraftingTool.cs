using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ActivateCraftingTool : MonoBehaviour
{
    public static ActivateCraftingTool Instance;
    [SerializeField] private GameObject Axe,Shovel,fishingRode,Hammer;
    public static bool handIsEmpty = true;
    [SerializeField] private Button takeAxeButton, takeShovelButton, takeFishingRodeButton, takeHammerButton,emptyHandButton,interectButton,ThrowButton,PullButton;
    public static bool isAxeActive, isShovelActive, isFishingRodeActive, isHammerActive,fishingStage1,fishingStage2,fishRodPull;
    private Animator anim;
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] FixedJoystick joystick;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
        takeFishingRodeButton.onClick.AddListener(ActivateFishingRode);
        takeAxeButton.onClick.AddListener(ActivatePickAxe);
        takeShovelButton.onClick.AddListener(ActivateShovel);
        takeHammerButton.onClick.AddListener(ActivateSword);
        interectButton.onClick.AddListener(onClickInterectButton);
        ThrowButton.onClick.AddListener(throwRod);
        PullButton.onClick.AddListener(pullRod);
        emptyHandButton.onClick.AddListener(onClickEmptyHand);
        setActiveAllToolsFalse();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            interectButton.onClick.Invoke();
            throwRod();
        }
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            takeAxeButton.onClick.Invoke();
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            takeShovelButton.onClick.Invoke();
        }
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            takeFishingRodeButton.onClick.Invoke();
        }
        if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            emptyHandButton.onClick.Invoke();
        }
        
        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
        {
            pullRod();
        }
        if (isFishingRodeActive)
        {
            interectButton.gameObject.SetActive(false);

            ThrowButton.gameObject.SetActive(true);
            PullButton.gameObject.SetActive(true);
        }
        else
        {
            interectButton.gameObject.SetActive(true);
            ThrowButton.gameObject.SetActive(false);
            PullButton.gameObject.SetActive(false);
        }
    }
    public void setActiveAllToolsFalse()
    {
        Axe.SetActive(false);
        Shovel.SetActive(false);
        fishingRode.SetActive(false);
        Hammer.SetActive(false);
        isAxeActive = false;
        isShovelActive = false;
        isFishingRodeActive = false;
        isHammerActive = false;
        fishRodPull=false;  
        handIsEmpty = true;
    }
    public void ActivatePickAxe()
    {
        setActiveAllToolsFalse();
        Axe.SetActive(true);
        isAxeActive = true;
        handIsEmpty = false;
        AudioManager.Instance.playSwitchSound();

    }
    public void ActivateShovel()
    {
        setActiveAllToolsFalse();
        Shovel.SetActive(true);
        isShovelActive = true;
        handIsEmpty = false;
        AudioManager.Instance.playSwitchSound();

    }
    public void ActivateFishingRode()
    {
        setActiveAllToolsFalse();
        fishingRode.SetActive(true);
        isFishingRodeActive = true;
        handIsEmpty = false;
        fishingStage1 = true;
        fishingStage2 = false;
        AudioManager.Instance.playSwitchSound();
    }
    public void ActivateSword()
    {
        setActiveAllToolsFalse();
        Hammer.SetActive(true);
        isHammerActive = true;
        handIsEmpty = false;
        AudioManager.Instance.playSwitchSound();
    }
    void onClickEmptyHand()
    {
        setActiveAllToolsFalse();
        CharacterMovement.instance.makeHandStateEmpty();
        AudioManager.Instance.playSwitchSound();
        
    }
    private void onClickInterectButton()
    {
        if(isAxeActive)
        {
            //do something with pickaxe
            anim.Play("Chopping");
            StartCoroutine(wait2());
        }
        else if (isShovelActive)
        {
            //do something with shovel
            anim.Play("Plowing");
            StartCoroutine(wait());
        }
        else if (isHammerActive)
        {
            anim.Play("Cutting");
            AudioManager.Instance.playNoHittingSound();
            //do something with hammer
        }
    }
    void throwRod()
    {
         if (isFishingRodeActive && FishingManager.canFishing)
        {
            //do something with fishing rode
            if (fishingStage1)
            {
                anim.Play("ThrowFishingRod");
                fishingStage1 = false;
                fishingStage2 = true;
                CharacterMovement.instance.freezePlayer();
                cam.gameObject.SetActive(false);
                joystick.enabled = false;
                AudioManager.Instance.playThrowingRodSound();
                FishingManager.Instance.bitingStart();
            }
        }
    }
    void pullRod()
    {
         if (isFishingRodeActive && FishingManager.canFishing)
        {
            
            if (fishingStage2)
            {
                FishingManager.Instance.stopBiting();
                anim.Play("PullOutRod");
                fishingStage2 = false;
                fishingStage1 = true; fishRodPull = true;
                CharacterMovement.instance.unfreezePlayer();
                cam.gameObject.SetActive(true);
                joystick.enabled = true;
            }
        }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.7f);

        AudioManager.Instance.playPlowingSound();

    }
    IEnumerator wait2()
    {
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.playNoHittingSound();
    }
    public bool isToolActive()
    {
        if(isAxeActive || isShovelActive || isFishingRodeActive || isHammerActive)
        {
            return true;
        }
        return false;
    }
}
