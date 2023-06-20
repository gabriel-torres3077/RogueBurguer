using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs: EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validIngredients;
    private List<KitchenObjectSO> ingredientList;
    private void Awake()
    {
        ingredientList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validIngredients.Contains(kitchenObjectSO))
        {
            return false;
        } else
        {

            ingredientList.Add(kitchenObjectSO);

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { kitchenObjectSO = kitchenObjectSO});

            return true;
        }
        
    }
    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return ingredientList;
    }

}
