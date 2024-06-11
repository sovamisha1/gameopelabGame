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
    }

    public KeyCode GetKeyForAction(string action)
    {
        return keyBindings.ContainsKey(action) ? keyBindings[action] : KeyCode.None;
    }
}
