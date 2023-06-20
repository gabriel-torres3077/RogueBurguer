using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;


    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // NO object on counter
            if (player.HasKitchenObject())
            {
                // Player carrying a object
                player.GetKitchenObject().SetKitchenObjectHolder(this);
            }
            else
            {
                //Player not carrying nothing
            }
        } else
        {
            if (player.HasKitchenObject())
            {
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // plate
                    plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectsSO());
                    GetKitchenObject().DestroySelf(); 
                }
                else
                {
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectsSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectHolder(player);
            }
        }
        
     
    }   

}
