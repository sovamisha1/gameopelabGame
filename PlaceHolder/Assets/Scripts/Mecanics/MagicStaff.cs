using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStaff : MonoBehaviour
{
    private Renderer staffRenderer;
    private bool isAtacking = false;
    private bool isHoldingStaff = false;

    void Start()
    {
        staffRenderer = gameObject.GetComponent<Renderer>();
    }

    public void SelectStaff(bool toDo)
    {
        isHoldingStaff = toDo;
        staffRenderer.enabled = toDo;
        HideAllChildren(gameObject, toDo);
    }

    public void Attack()
    {
        if (isHoldingStaff && !isAtacking)
        {
            Debug.Log("Атакую!");
        }
        else
        {
            Debug.Log("Держу посох: " + isHoldingStaff);
            Debug.Log("Уже атакую: " + isAtacking);
        }
    }

    public void HideAllChildren(GameObject parentObject, bool toDo)
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
}
