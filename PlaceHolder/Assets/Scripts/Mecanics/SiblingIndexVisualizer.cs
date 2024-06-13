using UnityEngine;

public class SiblingIndexVisualizer : MonoBehaviour
{
    private void Start()
    {
        Transform parentTransform = transform.parent;
        if (parentTransform != null)
        {
            for (int i = 0; i < parentTransform.childCount; i++)
            {
                Transform child = parentTransform.GetChild(i);
                int siblingIndex = child.GetSiblingIndex();
                Debug.Log($"{child.name} has sibling index {siblingIndex}");
            }
        }
        else
        {
            Debug.LogWarning("This object has no parent.");
        }
    }
}
