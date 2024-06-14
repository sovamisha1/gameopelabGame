using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : Interactable
{
    public GameObject craftingUI;
    private Inventory inventory;
    private CraftingUI craftingUIScreen;
    private PlayerController playerController;
    private CamController camController;
    private MoveCamera moveCamera;
    private Camera eventLettersCamera;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        craftingUIScreen = craftingUI.GetComponent<CraftingUI>();
        craftingUI.SetActive(false);

        playerController = FindObjectOfType<PlayerController>();
        moveCamera = FindObjectOfType<MoveCamera>();
        camController = FindObjectOfType<CamController>();
        eventLettersCamera = GameObject.Find("CraftingCamera").GetComponent<Camera>();
        eventLettersCamera.enabled = false;
    }

    public override void Interact()
    {
        craftingUI.SetActive(true);
        EnableCursor();
        DisablePlayerControl();
    }

    public void OnExit()
    {
        craftingUI.SetActive(false);
        DisableCursor();
        EnablePlayerControl();
    }

    void EnablePlayerControl()
    {
        if (playerController != null)
        {
            playerController.StopPlayer(false, eventLettersCamera);
            playerController.enabled = true;
        }
    }

    void DisablePlayerControl()
    {
        if (playerController != null)
        {
            playerController.StopPlayer(true, eventLettersCamera);
            playerController.enabled = false;
        }
    }

    void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
