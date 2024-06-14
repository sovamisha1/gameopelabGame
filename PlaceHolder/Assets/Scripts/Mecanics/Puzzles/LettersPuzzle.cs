using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LettersPuzzle : Interactable
{
    public InputField[] letterInputs;
    public Button submitButton;
    public Button exitButton;
    public GameObject puzzleUI;

    private string solution = "fate";

    private PlayerController playerController;
    private CamController camController;
    private MoveCamera moveCamera;
    private Camera eventLettersCamera;

    public override void Interact()
    {
        puzzleUI.SetActive(true);
        EnableCursor(); // Включаем курсор
        DisablePlayerControl(); // Отключаем управление игроком
    }

    // Начальная настройка
    void Start()
    {
        // Подключаем обработчики кнопок
        submitButton.onClick.AddListener(OnSubmit);
        exitButton.onClick.AddListener(OnExit);

        // Изначально мини-игра скрыта
        puzzleUI.SetActive(false);

        playerController = FindObjectOfType<PlayerController>();
        moveCamera = FindObjectOfType<MoveCamera>();
        camController = FindObjectOfType<CamController>();
        eventLettersCamera = GameObject.Find("LetterPuzleCamera").GetComponent<Camera>();
    }

    // Обработчик кнопки "Готово"
    void OnSubmit()
    {
        string input = "";
        for (int i = 0; i < letterInputs.Length; i++)
        {
            input += letterInputs[i].text.ToLower(); // собираем введенные буквы в строку
        }

        if (input == solution)
        {
            Debug.Log("Победа");
            puzzleUI.SetActive(false); // Скрываем UI канвас
            gameObject.layer = 0; // Устанавливаем слой на Default (0)
            Destroy(this); // Удаляем скрипт
            DisableCursor(); // Отключаем курсор
            EnablePlayerControl(); // Включаем управление игроком
        }
        else
        {
            ClearInputs();
            Debug.Log("Неправильный ответ");
        }
    }

    // Обработчик кнопки "Выход"
    void OnExit()
    {
        ClearInputs();
        puzzleUI.SetActive(false); // Скрываем UI канвас
        DisableCursor(); // Отключаем курсор
        EnablePlayerControl(); // Включаем управление игроком
    }

    // Метод для очистки всех полей ввода
    void ClearInputs()
    {
        foreach (var input in letterInputs)
        {
            input.text = "";
        }
    }

    // Включаем курсор
    void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Отключаем курсор
    void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Отключаем управление игроком
    void DisablePlayerControl()
    {
        if (playerController != null)
        {
            playerController.StopPlayer(true, eventLettersCamera); // eventLettersCamera - тип Camera
            playerController.enabled = false;
            //camController.enabled = false; //main камера отключается в StopPlayer
            //moveCamera.enabled = false;
        }
    }

    // Включаем управление игроком
    void EnablePlayerControl()
    {
        if (playerController != null)
        {
            playerController.StopPlayer(false, eventLettersCamera); // eventLettersCamera - тип Camera
            playerController.enabled = true;
            //camController.enabled = true; //main камера включается в StopPlayer
            //moveCamera.enabled = true;
        }
    }
}
