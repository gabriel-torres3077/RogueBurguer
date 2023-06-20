using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IProgress
{
    public event EventHandler<IProgress.OnProgressChangedEventArgs> OnProgressChanged;

    private enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private State state;

    private void Start()
    {
        state = State.Idle;
    }
    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Frying:
                fryingTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                {
                    progressNomalized = fryingTimer / fryingRecipeSO.fryingProgressMax
                });
                if (fryingTimer > fryingRecipeSO.fryingProgressMax)
                {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.kitchenObjectSpawn(fryingRecipeSO.output, this);
                    burningTimer = 0;
                    burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectsSO());
                    state = State.Fried;
                }
                break;
            case State.Fried:
                burningTimer += Time.deltaTime;

                OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                {
                    progressNomalized = burningTimer / burningRecipeSO.burningProgressMax
                });
                if (burningTimer > burningRecipeSO.burningProgressMax)
                {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.kitchenObjectSpawn(burningRecipeSO.output, this);
                    Debug.Log(state);
                    state = State.Burned;


                    OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                    {
                        progressNomalized = 0
                    });
                }
                break;
            case State.Burned:
                break;
        }
    }
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
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectsSO());

                    state = State.Frying;
                    fryingTimer = 0;

                    OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                    {
                        progressNomalized = fryingTimer / fryingRecipeSO.fryingProgressMax
                    });
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

                    state = State.Idle;

                    OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                    {
                        progressNomalized = 0
                    });
                }
            
        }
            else
            {
                // Player pickup item
                state = State.Idle;
                GetKitchenObject().SetKitchenObjectHolder(player);

                OnProgressChanged?.Invoke(this, new IProgress.OnProgressChangedEventArgs
                {
                    progressNomalized = 0
                });
            }
        }
    }
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO input)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(input);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        } else
        {
            return null;
        }
    }
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
