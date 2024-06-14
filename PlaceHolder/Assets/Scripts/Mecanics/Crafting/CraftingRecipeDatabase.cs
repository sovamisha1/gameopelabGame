using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class CraftingRecipeDatabase : MonoBehaviour
{
    public List<CraftingRecipe> craftingRecipes;

    void Awake()
    {
        craftingRecipes = new List<CraftingRecipe>
        {
            new CraftingRecipe()
            {
                itemName = "Пламя возрождения",
                itemSprite = Resources.Load<Sprite>("Imges/item1"),
                ingredients = new List<CraftingIngredient>
                {
                    new CraftingIngredient() { itemName = "Светлячок", quantity = 5 },
                    new CraftingIngredient()
                    {
                        itemName = "Кроваво-красный кристалл",
                        quantity = 3
                    },
                }
            },
            new CraftingRecipe()
            {
                itemName = "Сырая версия посоха (без Пера архангела и Нити Судьбы)",
                itemSprite = Resources.Load<Sprite>("Images/item2"),
                ingredients = new List<CraftingIngredient>
                {
                    new CraftingIngredient() { itemName = "Ветвь древа жизни", quantity = 1 },
                    new CraftingIngredient() { itemName = "Капли вечной воды", quantity = 1 },
                }
            },
            new CraftingRecipe()
            {
                itemName = "Тестовый крафт",
                itemSprite = Resources.Load<Sprite>("Imges/item3"),
                ingredients = new List<CraftingIngredient>
                {
                    new CraftingIngredient() { itemName = "Пламя возрождения", quantity = 1 },
                    new CraftingIngredient() { itemName = "ХАХАХАХАХХААХАХ", quantity = 30 },
                }
            }
        };
    }

    public List<CraftingRecipe> GetRecipes()
    {
        return craftingRecipes;
    }
}
