using UnityEngine;
using System.Collections;

public class Potion : MonoBehaviour
{
    public GameObject potion;
    public Camera mainCamera;
    public GameObject emptyPotionModel;
    public GameObject opendPotionModel;
    public GameObject fullPotionModel;
    public PlayerController playerController;

    private Animator animator;
    private bool canDrink = true;
    private bool takePotion = true;
    private bool canSwitch = true;
    private bool potionIsNotEmpty = true;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        if (potion == null)
            potion = this.gameObject;
        if (playerController == null)
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (animator == null)
            animator = potion.GetComponent<Animator>();
        if (emptyPotionModel == null)
            emptyPotionModel = GameObject.FindWithTag("PotionEmpty");
        if (fullPotionModel == null)
            fullPotionModel = GameObject.FindWithTag("PotionFull");
        if (opendPotionModel == null)
            opendPotionModel = GameObject.FindWithTag("PotionOpend");
        if (potion != null && mainCamera != null)
        {
            potion.transform.SetParent(mainCamera.transform, false);
            Positionpotion();
        }

        foreach (Transform child in potion.transform)
        {
            child.gameObject.GetComponent<Renderer>().enabled = takePotion;
        }

        potion.GetComponent<Renderer>().enabled = takePotion;
        RefilPotion();
    }

    void Update()
    {
        if (Input.GetKeyDown(InputManager.instance.GetKeyForAction("TakePotion")) && canSwitch)
        {
            SwitchPotion();
        }

        if (Input.GetKeyDown(InputManager.instance.GetKeyForAction("DrinkPotion")))
        {
            if (canDrink && potionIsNotEmpty)
            {
                StartCoroutine(DrinkPotion());
            }
        }
        if (canSwitch)
        {
            Positionpotion();
        }
        if (Input.GetKeyDown(InputManager.instance.GetKeyForAction("RefilPotion")))
        {
            if (!potionIsNotEmpty && potion.GetComponent<Renderer>().enabled == true)
            {
                RefilPotion();
            }
        }
    }

    IEnumerator DrinkPotion()
    {
        canSwitch = false;
        animator.SetBool("isStartedDrinking", true);
        HideAndShowFirstChild(false);
        SwitchPotionModel(gameObject, opendPotionModel);
        yield return new WaitForSeconds(1.25f);
        animator.SetBool("isStartedDrinking", false);
        yield return new WaitForSeconds(1f);
        SwitchPotionModel(gameObject, emptyPotionModel);
        playerController.UseHeal(100f);
        animator.SetBool("isFinishedDrinking", true);
        yield return new WaitForSeconds(1.25f);
        animator.SetBool("isFinishedDrinking", false);
        Positionpotion();
        canSwitch = true;
        potionIsNotEmpty = false;
        yield break;
    }

    void RefilPotion()
    {
        SwitchPotionModel(gameObject, fullPotionModel);
        HideAndShowFirstChild(true);
        HideAndSecondFirstChild(true);
        potionIsNotEmpty = true;
    }

    void SwitchPotion()
    {
        canDrink = !canDrink;
        takePotion = !takePotion;
        if (potionIsNotEmpty)
            HideAndShowFirstChild(takePotion);
        HideAndSecondFirstChild(takePotion);
        potion.GetComponent<Renderer>().enabled = takePotion;
    }

    void SwitchPotionModel(GameObject fromModel, GameObject toModel)
    {
        MeshFilter mf1 = fromModel.GetComponent<MeshFilter>();
        MeshRenderer mr1 = fromModel.GetComponent<MeshRenderer>();
        MeshFilter mf2 = toModel.GetComponent<MeshFilter>();
        MeshRenderer mr2 = toModel.GetComponent<MeshRenderer>();

        mf1.mesh = mf2.mesh;
        mr1.materials = mr2.materials;
    }

    void HideAndShowFirstChild(bool toDo)
    {
        Transform firstChild = transform.GetChild(0);
        firstChild.gameObject.GetComponent<Renderer>().enabled = toDo;
    }

    void HideAndSecondFirstChild(bool toDo)
    {
        Transform secondChild = transform.GetChild(1);
        secondChild.gameObject.GetComponent<Renderer>().enabled = toDo;
    }

    void Positionpotion()
    {
        Vector3 localPosition = new Vector3(-0.95f, -0.6f, 1.4f);
        potion.transform.localPosition = localPosition;
        potion.transform.localRotation = Quaternion.Euler(-90, 0, 0);
    }
}
