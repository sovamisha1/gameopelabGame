using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public GameObject keyBindingParent; // Parent for key binding entries
    public GameObject keyBindingPrefab; // Prefab for the key binding entries

    void Start()
    {
        // Установить значение ползунка
        // volumeSlider.value = AudioListener.volume;

        // Создаем элементы для настройки клавиш
        CreateKeyBindingEntries();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void SetKeyBinding(string action)
    {
        StartCoroutine(WaitForKeyPress(action));
    }

    private IEnumerator WaitForKeyPress(string action)
    {
        yield return new WaitUntil(() => Input.anyKeyDown);

        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                InputManager.instance.SetKeyForAction(action, keyCode);
                RefreshKeyBindingEntries(); // Обновляем отображение, чтобы отразить новое назначение
                break;
            }
        }
    }

    private void CreateKeyBindingEntries()
    {
        foreach (KeyValuePair<string, KeyCode> entry in InputManager.instance.GetKeyBindings())
        {
            GameObject newEntry = Instantiate(keyBindingPrefab, keyBindingParent.transform);
            newEntry.transform.Find("ActionNameText").GetComponent<Text>().text = entry.Key;
            newEntry.transform.Find("KeyBindButton").GetComponentInChildren<Text>().text =
                entry.Value.ToString();

            newEntry.transform
                .Find("KeyBindButton")
                .GetComponent<Button>()
                .onClick.AddListener(() => SetKeyBinding(entry.Key));
        }
    }

    private void RefreshKeyBindingEntries()
    {
        foreach (Transform child in keyBindingParent.transform)
        {
            Destroy(child.gameObject);
        }
        CreateKeyBindingEntries();
    }
}
