using UnityEngine;
using UnityEngine.UI;

public class GroceryList : MonoBehaviour
{
    public Text groceryListText;
    public string[] groceryItems = { "Milk", "Cereal", "Chips", "Fruit", "Vegetables", "Soup", "Soda", "Pizza", "Toilet Paper", "Bread" };
    //GroceryList.crossedOffItems[0] = true;
    public static bool[] crossedOffItems = new bool[10];

    private string StrikeThrough(string s)
    {
        string strikethrough = "";
        foreach (char c in s)
        {
            strikethrough = strikethrough + c + '\u0336';
        }
        return strikethrough;
    }

    private string[] RandomizeArray(string[] array)
    {
        string[] randomizedArray = array.Clone() as string[];
        for (int i = 0; i < randomizedArray.Length; i++)
        {
            string temp = randomizedArray[i];
            int randomIndex = Random.Range(i, randomizedArray.Length);
            randomizedArray[i] = randomizedArray[randomIndex];
            randomizedArray[randomIndex] = temp;
        }

        return randomizedArray;
    }

    private void Start()
    {
        // Generate a random list of groceries
        string groceryList = "";
        string[] randomizedGroceries = RandomizeArray(groceryItems);
        for (int i = 0; i < 5; i++)
        {
            groceryList += randomizedGroceries[i] + "\n";
        }

        // Update the text of the grocery list in the HUD
        groceryListText.text = groceryList;
    }

    private void Update()
    {
        // Check if any items have been crossed off
        for (int i = 0; i < crossedOffItems.Length; i++)
        {
            if (crossedOffItems[i])
            {
                string crossedOffItem = groceryItems[i];
                groceryListText.text = groceryListText.text.Replace(crossedOffItem, StrikeThrough(crossedOffItem));
                //groceryListText.color = Color.red;
            }
        }
    }
}
