using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CraftgUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Transform recipeButtonParent;
    public Transform ingredientParent;
    public Transform ingredientForRecipeParent;
    public GameObject recipeButtonPrefab;
    public GameObject ingredientPrefab;
    public GameObject ingredientForRecipePrefab;
    public Button craftButton;
    public Button exitButton;
    public Text nameText;
    public Text descripion;
    public Image craftedItemImage;

    private List<CraftingRecipe> recipes;
    private List<CraftingIngredient> items;
    private Inventory inventory;
    private Crafting currentCrafting;
    private CraftingRecipe selectedRecipe;
    private CraftingIngredient selectedIngredient;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        currentCrafting = FindObjectOfType<Crafting>();
        craftButton.onClick.AddListener(TryCraftItem);
        exitButton.onClick.AddListener(currentCrafting.OnExit);

        var recipeDatabase = FindObjectOfType<CraftingRecipeDatabase>();
        recipes = recipeDatabase != null ? recipeDatabase.GetRecipes() : new List<CraftingRecipe>();

        var itemsDatabase = FindObjectOfType<CraftingIngredientsDatabase>();
        items =
            itemsDatabase != null ? itemsDatabase.GetIngredients() : new List<CraftingIngredient>();

        ShowEverything();
        SelectThis(items[0]);
    }

    void TryCraftItem()
    {
        if (HasRequiredItems(selectedRecipe.ingredients))
        {
            RemoveRequiredItems(selectedRecipe.ingredients);
            ShowIngredientsForRecipe(true);
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

    void RemoveRequiredItems(List<CraftingIngredient> ingredients)
    {
        foreach (var ingredient in ingredients)
        {
            inventory.RemoveItem(ingredient.itemName, ingredient.quantity);
        }
    }

    void HighlightMissingIngredients()
    {
        foreach (Transform child in ingredientForRecipeParent)
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
        ShowIngredientsForRecipe(true);
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

    void SelectThis(CraftingIngredient item)
    {
        selectedIngredient = item;
        selectedRecipe = null;
        craftButton.enabled = false;
        ShowCard(item);
        ShowIngredientsForRecipe(false);
    }

    void SelectThis(CraftingRecipe recipe)
    {
        selectedRecipe = recipe;
        selectedIngredient = null;
        craftButton.enabled = true;
        ShowCard(recipe);
        ShowIngredientsForRecipe(true);
    }

    void ShowCard(CraftingIngredient item)
    {
        nameText.text = item.itemName;
        descripion.text = item.itemDesc;
    }

    void ShowCard(CraftingRecipe recipe)
    {
        nameText.text = recipe.itemName;
        descripion.text = recipe.itemDescription;
    }

    void ShowIngredientsForRecipe(bool needToShow)
    {
        foreach (Transform child in ingredientForRecipeParent)
        {
            Destroy(child.gameObject);
        }
        if (!needToShow)
        {
            return;
        }
        foreach (var ingredient in selectedRecipe.ingredients)
        {
            GameObject ingredientObj = Instantiate(
                ingredientForRecipePrefab,
                ingredientForRecipeParent
            );
            Text ingredientText = ingredientObj.GetComponentInChildren<Text>();

            int playerAmount = inventory.DoesContainItem(ingredient.itemName)
                ? inventory.inventory[ingredient.itemName]
                : 0;
            ingredientText.text = $"{ingredient.itemName} {playerAmount}/{ingredient.quantity}";

            ingredientText.color = playerAmount >= ingredient.quantity ? Color.green : Color.red;
        }
    }

    void ShowEverything()
    {
        foreach (Transform child in recipeButtonParent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in ingredientParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var recipe in recipes)
        {
            GameObject buttonObj = Instantiate(recipeButtonPrefab, recipeButtonParent);
            Button button = buttonObj.GetComponent<Button>();
            button.GetComponentInChildren<Text>().text = recipe.itemName;
            button.onClick.AddListener(() => SelectThis(recipe));
        }
        foreach (var item in items)
        {
            GameObject buttonObj = Instantiate(recipeButtonPrefab, ingredientParent);
            Button button = buttonObj.GetComponent<Button>();
            button.GetComponentInChildren<Text>().text = item.itemName;
            button.onClick.AddListener(() => SelectThis(item));
        }
    }
}
