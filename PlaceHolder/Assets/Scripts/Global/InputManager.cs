using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    private Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadKeyBindings();
    }

    private void LoadKeyBindings()
    {
        keyBindings["Flash"] = (KeyCode)
            System.Enum.Parse(
                typeof(KeyCode),
                PlayerPrefs.GetString("Flash", KeyCode.Mouse1.ToString())
            );
        keyBindings["Jump"] = (KeyCode)
            System.Enum.Parse(
                typeof(KeyCode),
                PlayerPrefs.GetString("Jump", KeyCode.Space.ToString())
            );
        keyBindings["Recharge"] = (KeyCode)
            System.Enum.Parse(
                typeof(KeyCode),
                PlayerPrefs.GetString("Recharge", KeyCode.R.ToString())
            );
        keyBindings["Run"] = (KeyCode)
            System.Enum.Parse(
                typeof(KeyCode),
                PlayerPrefs.GetString("Run", KeyCode.LeftShift.ToString())
            );
        keyBindings["Interact1"] = (KeyCode)
            System.Enum.Parse(
                typeof(KeyCode),
                PlayerPrefs.GetString("Interact1", KeyCode.Alpha1.ToString())
            );
        keyBindings["Interact2"] = (KeyCode)
            System.Enum.Parse(
                typeof(KeyCode),
                PlayerPrefs.GetString("Interact2", KeyCode.Alpha2.ToString())
            );
        keyBindings["UseItem"] = (KeyCode)
            System.Enum.Parse(
                typeof(KeyCode),
                PlayerPrefs.GetString("UseItem", KeyCode.Mouse0.ToString())
            );
        keyBindings["RefilPotion"] = (KeyCode)
            System.Enum.Parse(
                typeof(KeyCode),
                PlayerPrefs.GetString("RefilPotion", KeyCode.N.ToString())
            );
        keyBindings["Crouch"] = (KeyCode)
            System.Enum.Parse(
                typeof(KeyCode),
                PlayerPrefs.GetString("Crouch", KeyCode.LeftControl.ToString())
            );
        keyBindings["Interact"] = (KeyCode)
            System.Enum.Parse(
                typeof(KeyCode),
                PlayerPrefs.GetString("Interact", KeyCode.E.ToString())
            );
    }

    public void SetKeyForAction(string action, KeyCode key)
    {
        keyBindings[action] = key;
        PlayerPrefs.SetString(action, key.ToString());
    }

    public KeyCode GetKeyForAction(string action)
    {
        return keyBindings.ContainsKey(action) ? keyBindings[action] : KeyCode.None;
    }

    public Dictionary<string, KeyCode> GetKeyBindings()
    {
        return keyBindings;
    }
}
