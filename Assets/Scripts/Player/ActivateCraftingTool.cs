using UnityEngine;
using UnityEngine.UI;

public class ActivateCraftingTool : MonoBehaviour
{
    [SerializeField] private GameObject PickAxe,Shovel,fishingRode,Hammer;
    public static bool handIsEmpty = true;
    [SerializeField] private Button takePickAxeButton, takeShovelButton, takeFishingRodeButton, takeHammerButton,interectButton;
    private bool isPickAxeActive, isShovelActive, isFishingRodeActive, isHammerActive;
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        takeFishingRodeButton.onClick.AddListener(ActivateFishingRode);
        takePickAxeButton.onClick.AddListener(ActivatePickAxe);
        takeShovelButton.onClick.AddListener(ActivateShovel);
        takeHammerButton.onClick.AddListener(ActivateSword);
        interectButton.onClick.AddListener(onClickInterectButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void setActiveAllToolsFalse()
    {
        PickAxe.SetActive(false);
        Shovel.SetActive(false);
        fishingRode.SetActive(false);
        Hammer.SetActive(false);
        isPickAxeActive = false;
        isShovelActive = false;
        isFishingRodeActive = false;
        isHammerActive = false;
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
    }
    public void ActivateSword()
    {
        setActiveAllToolsFalse();
        Hammer.SetActive(true);
        isHammerActive = true;
        handIsEmpty = false;
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
        else if (isFishingRodeActive)
        {
            //do something with fishing rode
            anim.Play("ThrowFishingRod");
        }
        else if (isHammerActive)
        {
            anim.Play("Cutting");
            //do something with hammer
        }
    }
}
