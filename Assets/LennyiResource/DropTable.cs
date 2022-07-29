using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DropTable<T>
{
    public List<DropTableItem<T>> items;
    List<int> _cachedLoot = new List<int>();

    //If you dont want this to be shown, just remove "DisplayWihtoutEdit" attribute
#if UNITY_EDITOR 
    [DisplayWithoutEdit] 
    #endif 
    public uint totalAmount;

    //REMEMBER TO CALL THIS IN ONVALIDATE, AWAKE, (and instead of Awake, use OnEnable for ScriptableObjects)
    public void Init()
    {
        _cachedLoot.Clear();

        int x = 0;
        foreach (var item in items)
        {
            for (int i = 0; i < item.weight; i++)
                _cachedLoot.Add(x);
            x++;
        }

        //Sets the total amount to be... The total amount.
        totalAmount = (uint)_cachedLoot.Count;

        foreach (var item in items) //Displays the calculated percentage
        {
            item._percentageChance = CalculatePercentage(item.weight);
            item.name = item.item.ToString();
        }
    }

    string CalculatePercentage(uint weight)
    {
        //Calculates the displayed percentage
        return $"{Math.Round((float) weight / totalAmount * 100, 2)}%";
    }

    public T GetItem() => //Returns a random item from all of the items
        items.Count > 0 ? items[_cachedLoot[UnityEngine.Random.Range(0, _cachedLoot.Count)]].item : default;
}

[Serializable]
public class DropTableItem<T>
{
    [DisplayWithoutEdit] public string name;
    public T item;
    public uint weight = 1;
    [DisplayWithoutEdit] public string _percentageChance;
}