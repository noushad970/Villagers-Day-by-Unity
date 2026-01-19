using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class ActivateCraftingTool : MonoBehaviour
{
    public static ActivateCraftingTool Instance;
    [SerializeField] private GameObject PickAxe,Shovel,fishingRode,Hammer;
    public static bool handIsEmpty = true;
    [SerializeField] private Button takePickAxeButton, takeShovelButton, takeFishingRodeButton, takeHammerButton,emptyHandButton,interectButton;
    public static bool isPickAxeActive, isShovelActive, isFishingRodeActive, isHammerActive,fishingStage1,fishingStage2,fishRodPull;
    private Animator anim;
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] FixedJoystick joystick;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        anim = GetComponent<Animator>();
        takeFishingRodeButton.onClick.AddListener(ActivateFishingRode);
        takePickAxeButton.onClick.AddListener(ActivatePickAxe);
        takeShovelButton.onClick.AddListener(ActivateShovel);
        takeHammerButton.onClick.AddListener(ActivateSword);
        interectButton.onClick.AddListener(onClickInterectButton);
        emptyHandButton.onClick.AddListener(onClickEmptyHand);
        setActiveAllToolsFalse();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setActiveAllToolsFalse()
    {
        PickAxe.SetActive(false);
        Shovel.SetActive(false);
        fishingRode.SetActive(false);
        Hammer.SetActive(false);
        isPickAxeActive = false;
        isShovelActive = false;
        isFishingRodeActive = false;
        isHammerActive = false;
        fishRodPull=false;  
        handIsEmpty = true;
    }
    public void ActivatePickAxe()
    {
        setActiveAllToolsFalse();
        PickAxe.SetActive(true);
        isPickAxeActive = true;
        handIsEmpty = false;

    }
    public void ActivateShovel()
    {
        setActiveAllToolsFalse();
        Shovel.SetActive(true);
        isShovelActive = true;
        handIsEmpty = false;

    }
    public void ActivateFishingRode()
    {
        setActiveAllToolsFalse();
        fishingRode.SetActive(true);
        isFishingRodeActive = true;
        handIsEmpty = false;
        fishingStage1 = true;
        fishingStage2 = false;
    }
    public void ActivateSword()
    {
        setActiveAllToolsFalse();
        Hammer.SetActive(true);
        isHammerActive = true;
        handIsEmpty = false;
    }
    void onClickEmptyHand()
    {
        setActiveAllToolsFalse();
        CharacterMovement.instance.makeHandStateEmpty();
    }
    private void onClickInterectButton()
    {
        if(isPickAxeActive)
        {
            //do something with pickaxe
            anim.Play("Mining");
        }
        else if (isShovelActive)
        {
            //do something with shovel
            anim.Play("Plowing");
        }
        else if (isFishingRodeActive && FishingManager.canFishing)
        {
            //do something with fishing rode
            if (fishingStage1)
            {
                anim.Play("ThrowFishingRod");
                fishingStage1 = false;
                fishingStage2 = true;
                CharacterMovement.instance.freezePlayer();
                cam.gameObject.SetActive(false);
                joystick.enabled=false;
                
                FishingManager.Instance.bitingStart();
            }
            else if (fishingStage2)
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
        else if (isHammerActive)
        {
            anim.Play("Cutting");
            //do something with hammer
        }
    }
    public bool isToolActive()
    {
        if(isPickAxeActive || isShovelActive || isFishingRodeActive || isHammerActive)
        {
            return true;
        }
        return false;
    }
}
