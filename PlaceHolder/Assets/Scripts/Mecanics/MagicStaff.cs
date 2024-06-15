using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStaff : MonoBehaviour
{
    private Renderer staffRenderer;
    private Animator animator;
    public GameObject magicProjectilePrefab;

    private bool isAtacking = false;
    private bool isHoldingStaff = false;

    // Префаб снаряда.
    public float projectileSpeed = 20.0f;
    public float projectileLifetime = 5.0f;

    void Start()
    {
        staffRenderer = gameObject.GetComponent<Renderer>();
        staffRenderer.enabled = false;
        animator = gameObject.GetComponent<Animator>();
        HideAllChildren(gameObject, false);
    }

    public void SelectStaff(bool toDo)
    {
        isHoldingStaff = toDo;
        staffRenderer.enabled = toDo;
        HideAllChildren(gameObject, toDo);
    }

    public void TryToAttack()
    {
        if (isHoldingStaff && !isAtacking)
        {
            StartCoroutine(Atack());
        }
        else
        {
            Debug.Log("Держу посох: " + isHoldingStaff);
            Debug.Log("Уже атакую: " + isAtacking);
        }
    }

    IEnumerator Atack()
    {
        isAtacking = true;
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(1f);
        ThrowMagic();
        animator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(1f);
        isAtacking = false;
        yield break;
    }

    private void ThrowMagic()
    {
        Vector3 staffPosition = transform.position;

        GameObject projectileInstance = Instantiate(
            magicProjectilePrefab,
            staffPosition,
            Quaternion.identity
        );

        Camera mainCamera = Camera.main;
        Ray cameraRay = mainCamera.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2)
        );
        Vector3 direction = cameraRay.direction;
        Rigidbody projectileRigidbody = projectileInstance.GetComponent<Rigidbody>();
        if (projectileRigidbody != null)
        {
            projectileRigidbody.velocity = direction * projectileSpeed;
        }

        Destroy(projectileInstance, projectileLifetime);
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
