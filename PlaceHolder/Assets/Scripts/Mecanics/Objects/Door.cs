using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoorColntroller;

public class Door : Interactable
{
    public KeyType keyType;
    private Inventory inventory;
    private Animator animator;

    private bool isOpend = true;

    private Dictionary<KeyType, string> dict = new Dictionary<KeyType, string>
    {
        { KeyType.None, "none" },
        { KeyType.Red, "Красный ключь" },
        { KeyType.Blue, "Синий ключь" }
    };

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        animator = transform.parent.GetComponent<Animator>();
    }

    public override void Interact()
    {
        if (keyType != KeyType.None)
        {
            string keyName;
            if (dict.TryGetValue(keyType, out keyName))
            {
                if (inventory.DoesContainItem(keyName))
                {
                    StartCoroutine(OpenAndCloseDoor());
                }
                else
                {
                    Debug.Log("У вас нет ключа");
                    StartCoroutine(ShakeDoor());
                }
            }
            else
            {
                Debug.Log("Тип ключа не найден.");
            }
        }
        else
        {
            StartCoroutine(OpenAndCloseDoor());
        }
    }

    IEnumerator OpenAndCloseDoor()
    {
        animator.SetBool("isOpend", isOpend);
        isOpend = !isOpend;
        yield return new WaitForSeconds(0.70f);
        yield break;
    }

    IEnumerator ShakeDoor()
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsedTime = 0f;
        float duration = 0.5f; // Duration of the shake
        float magnitude = 0.1f; // Magnitude of the shake

        while (elapsedTime < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(
                originalPosition.x + x,
                originalPosition.y + y,
                originalPosition.z
            );

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
