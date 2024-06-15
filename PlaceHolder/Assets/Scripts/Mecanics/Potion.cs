using UnityEngine;
using System.Collections;

public class Potion : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject potion;
    public GameObject emptyPotionModel;
    public GameObject opendPotionModel;
    public GameObject fullPotionModel;
    public Renderer potionRenderer;
    public PlayerController playerController;

    private Animator animator;

    private bool isHoldingPotion = false;
    private bool isNotEmptyPotion = true;
    private bool isDrinkingPotion = false;

    void Start()
    {
        isHoldingPotion = false;
        isNotEmptyPotion = true;
        isDrinkingPotion = false;
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
        }
        potionRenderer = potion.GetComponent<Renderer>();
        potionRenderer.enabled = isHoldingPotion;
        RefilPotion();
        HideAndShowFirstChild(false);
        HideAndShowSecondChild(false);
    }

    void Update()
    {
        if (isHoldingPotion)
        {
            Positionpotion();
        }
    }

    IEnumerator DrinkPotion()
    {
        isDrinkingPotion = true;
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
        isNotEmptyPotion = false;
        isDrinkingPotion = false;
        yield break;
    }

    public void RefilPotion()
    {
        SwitchPotionModel(gameObject, fullPotionModel);
        HideAndShowFirstChild(true);
        HideAndShowSecondChild(true);
        isNotEmptyPotion = true;
    }

    public void TryToDrink()
    {
        if (isHoldingPotion && isNotEmptyPotion && isDrinkingPotion == false)
            StartCoroutine(DrinkPotion());
        else
        {
            Debug.Log("Держу зелье: " + isHoldingPotion);
            Debug.Log("Зелье не пустое: " + isNotEmptyPotion);
            Debug.Log("Уже пью: " + isDrinkingPotion);
        }
    }

    public void SelectPotion(bool toDo)
    {
        if (isDrinkingPotion)
            return;
        if (isNotEmptyPotion)
        {
            HideAndShowSecondChild(toDo);
            HideAndShowFirstChild(toDo);
        }
        else
        {
            HideAndShowSecondChild(toDo);
        }
        isHoldingPotion = toDo;
        potionRenderer.enabled = toDo;
    }

    private void HideAllChildren(GameObject parentObject, bool toDo)
    {
        foreach (Transform child in parentObject.transform)
        {
            Renderer childRenderer = child.gameObject.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                childRenderer.enabled = toDo;
            }
            if (child.childCount > 0)
            {
                HideAllChildren(child.gameObject, toDo);
            }
        }
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

    private void HideAndShowFirstChild(bool toDo)
    {
        Transform firstChild = transform.GetChild(0);
        firstChild.gameObject.GetComponent<Renderer>().enabled = toDo;
    }

    private void HideAndShowSecondChild(bool toDo)
    {
        Transform firstChild = transform.GetChild(1);
        firstChild.gameObject.GetComponent<Renderer>().enabled = toDo;
    }

    void Positionpotion()
    {
        Vector3 localPosition = new Vector3(-0.2f, -0.15f, 0.34f);
        potion.transform.localPosition = localPosition;
        potion.transform.localRotation = Quaternion.Euler(-90, 0, 0);
    }
}
