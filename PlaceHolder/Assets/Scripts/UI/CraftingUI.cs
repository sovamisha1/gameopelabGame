using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CraftingUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject craftingPanel;
    public Transform recipeButtonParent;
    public Transform ingredientParent;
    public GameObject recipeButtonPrefab;
    public GameObject ingredientPrefab;
    public Button craftButton;
    public Button exitButton;
    public Image craftedItemImage;

    private List<CraftingRecipe> recipes;
    private Inventory inventory;
    private Crafting currentCrafting;
    private CraftingRecipe selectedRecipe;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        currentCrafting = FindObjectOfType<Crafting>();

        craftButton.onClick.AddListener(TryCraftItem);
        exitButton.onClick.AddListener(currentCrafting.OnExit);

        var recipeDatabase = FindObjectOfType<CraftingRecipeDatabase>();
        recipes = recipeDatabase != null ? recipeDatabase.GetRecipes() : new List<CraftingRecipe>();

        HideCraftedItemImage();

        PopulateRecipeButtons();
    }

    void PopulateRecipeButtons()
    {
        foreach (Transform child in recipeButtonParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var recipe in recipes)
        {
            GameObject buttonObj = Instantiate(recipeButtonPrefab, recipeButtonParent);
            Button button = buttonObj.GetComponent<Button>();
            button.GetComponentInChildren<Text>().text = recipe.itemName;
            button.onClick.AddListener(() => SelectRecipe(recipe));
        }
    }

    void SelectRecipe(CraftingRecipe recipe)
    {
        selectedRecipe = recipe;
        PopulateIngredientList();
    }

    void PopulateIngredientList()
    {
        foreach (Transform child in ingredientParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var ingredient in selectedRecipe.ingredients)
        {
            GameObject ingredientObj = Instantiate(ingredientPrefab, ingredientParent);
            Text ingredientText = ingredientObj.GetComponentInChildren<Text>();

            int playerAmount = inventory.DoesContainItem(ingredient.itemName)
                ? inventory.inventory[ingredient.itemName]
                : 0;
            ingredientText.text = $"{ingredient.itemName} {playerAmount}/{ingredient.quantity}";

            ingredientText.color = playerAmount >= ingredient.quantity ? Color.green : Color.red;
        }
    }

    void TryCraftItem()
    {
        if (selectedRecipe == null)
        {
            Debug.LogWarning("No recipe selected!");
            return;
        }

        if (HasRequiredItems(selectedRecipe.ingredients))
        {
            RemoveRequiredItems(selectedRecipe.ingredients);
            PopulateIngredientList();
            inventory.AddItem(selectedRecipe.itemName);
            craftedItemImage.sprite = selectedRecipe.itemSprite;
            craftedItemImage.gameObject.SetActive(true);
            if (selectedRecipe.itemSprite != null)
            {
                craftedItemImage.sprite = selectedRecipe.itemSprite;
                Debug.Log("Sprite поменялся!");
            }
            else
            {
                Debug.Log("Sprite не найден!");
            }
            Invoke("HideCraftedItemImage", 1f);
        }
        else
        {
            HighlightMissingIngredients();
        }
    }

    bool HasRequiredItems(List<CraftingIngredient> ingredients)
    {
        foreach (var ingredient in ingredients)
        {
            if (
                !inventory.DoesContainItem(ingredient.itemName)
                || inventory.inventory[ingredient.itemName] < ingredient.quantity
            )
            {
                return false;
            }
        }
        return true;
    }

    void RemoveRequiredItems(List<CraftingIngredient> ingredients)
    {
        foreach (var ingredient in ingredients)
        {
            inventory.RemoveItem(ingredient.itemName, ingredient.quantity);
        }
    }

    void HideCraftedItemImage()
    {
        craftedItemImage.gameObject.SetActive(false);
    }

    void HighlightMissingIngredients()
    {
        foreach (Transform child in ingredientParent)
        {
            Text ingredientText = child.GetComponentInChildren<Text>();
            if (ingredientText.color == Color.red)
            {
                ingredientText.color = Color.yellow;
                Invoke("ResetIngredientTextColors", 1f);
            }
        }
    }

    void ResetIngredientTextColors()
    {
        PopulateIngredientList();
    }

    List<CraftingRecipe> LoadRecipes()
    {
        // Load your recipes here (e.g., from ScriptableObject, JSON, etc.)
        return new List<CraftingRecipe>();
    }
}
