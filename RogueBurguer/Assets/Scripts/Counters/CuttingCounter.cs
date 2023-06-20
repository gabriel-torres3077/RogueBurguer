using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IProgress
{
    public event EventHandler<IProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    public KitchenObjectSO cutkitchenObjectSO;

    private int cuttingProgress;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // NO object on counter
            if (player.HasKitchenObject())
            {
                // Player carrying a object
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectsSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectHolder(this);
                    cuttingProgress = 0;
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectsSO());

                    OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                    {
                        progressNomalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    }) ;

                }
            }
            else
            {
                //Player not carrying nothing
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // plate
                    plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectsSO());
                    GetKitchenObject().DestroySelf();
                }
            
            }
            else
            {
                GetKitchenObject().SetKitchenObjectHolder(player);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == kitchenObjectSO)
            {
                return true;
            }
        }
        return false;
    }
    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectsSO())) 
        {

            cuttingProgress++;

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectsSO());

            OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
            {
                progressNomalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectsSO());

                GetKitchenObject().DestroySelf();

                KitchenObject.kitchenObjectSpawn(outputKitchenObjectSO, this);
            }
        }
    }
    private bool HasRecipeObjectSO(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO input)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(input);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach(CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if(cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
