using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingIngredientsDatabase : MonoBehaviour
{
    public List<CraftingIngredient> craftingIngredients;

    void Awake()
    {
        craftingIngredients = new List<CraftingIngredient>
        {
            new CraftingIngredient() { itemName = "Светлячки", itemDesc = "Заебался писать UI61" },
            new CraftingIngredient() { itemName = "Кроваво - красный кристалл", itemDesc = "Заебался писать UI62" },
            new CraftingIngredient() { itemName = "Капли вечной воды", itemDesc = "Заебался писать UI63" },
            new CraftingIngredient() { itemName = "Перо Архангела", itemDesc = "Заебался писать UI64" },
            new CraftingIngredient() { itemName = "Нить судьбы", itemDesc = "Заебался писать UI65" },
            new CraftingIngredient() { itemName = "Ромашка", itemDesc = "Заебался писать UI6" }
        };
    }

    public List<CraftingIngredient> GetIngredients()
    {
        return craftingIngredients;
    }
}
