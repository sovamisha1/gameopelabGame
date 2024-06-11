using UnityEngine;
using System.Collections;

public class Potion : MonoBehaviour
{
    public GameObject potion;
    public Camera mainCamera;
    public GameObject emptyPotionModel;
    public GameObject fullPotionModel;

    private bool canDrink = false;
    private bool takePotion = false;
    private bool canSwitch = true;
    private bool potionIsNotEmpty = true;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        if (potion == null)
            potion = this.gameObject;
        if (potion != null && mainCamera != null)
        {
            potion.transform.parent = mainCamera.transform;
            Positionpotion();
        }

        foreach (Transform child in potion.transform)
        {
            child.gameObject.GetComponent<Renderer>().enabled = takePotion;
        }
        potion.GetComponent<Renderer>().enabled = takePotion;
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
            if (!potionIsNotEmpty)
            {
                RefilPotion();
            }
        }
    }

    IEnumerator DrinkPotion()
    {
        canSwitch = false;
        Vector3 screenPoint = new Vector3(Screen.width - 2200, 800, mainCamera.nearClipPlane + 1);
        potion.transform.position = mainCamera.ScreenToWorldPoint(screenPoint);
        HideAndShowFirstChild(1);
        yield return new WaitForSeconds(2);
        SwitchPotionModel(gameObject, emptyPotionModel);
        canSwitch = true;
        potionIsNotEmpty = false;
        yield break;
    }

    void RefilPotion()
    {
        SwitchPotionModel(gameObject, fullPotionModel);
        HideAndShowFirstChild(0);
        potionIsNotEmpty = true;
    }

    void SwitchPotion()
    {
        canDrink = !canDrink;
        takePotion = !takePotion;
        foreach (Transform child in potion.transform)
        {
            child.gameObject.GetComponent<Renderer>().enabled = takePotion;
        }
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

    public void HideAndShowFirstChild(int toDo)
    {
        if (transform.childCount > 0)
        {
            Transform firstChild = transform.GetChild(0);

            if (firstChild != null && toDo == 1)
            {
                firstChild.gameObject.GetComponent<Renderer>().enabled = false;
            }
            else if (firstChild != null && toDo == 0)
            {
                firstChild.gameObject.GetComponent<Renderer>().enabled = takePotion;
            }
        }
    }

    void Positionpotion()
    {
        Vector3 screenPoint = new Vector3(Screen.width - 2200, 200, mainCamera.nearClipPlane + 1);
        potion.transform.position = mainCamera.ScreenToWorldPoint(screenPoint);
        // potion.transform.rotation = mainCamera.transform.rotation; // угол
    }
}
