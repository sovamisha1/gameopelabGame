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
        // Дефолтные значения ключей
        keyBindings["Flash"] = KeyCode.Mouse1;
        keyBindings["Jump"] = KeyCode.Space;
        keyBindings["Recharge"] = KeyCode.R;
        keyBindings["Run"] = KeyCode.LeftShift;
        keyBindings["Interact1"] = KeyCode.Alpha1;
        keyBindings["Interact2"] = KeyCode.Alpha2;
        keyBindings["UseItem"] = KeyCode.Mouse0;
        keyBindings["RefilPotion"] = KeyCode.N;
        keyBindings["Crouch"] = KeyCode.LeftControl;
        keyBindings["Interact"] = KeyCode.E;
    }

    public KeyCode GetKeyForAction(string action)
    {
        return keyBindings.ContainsKey(action) ? keyBindings[action] : KeyCode.None;
    }
}
