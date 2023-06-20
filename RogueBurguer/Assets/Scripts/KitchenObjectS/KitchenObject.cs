using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectHolder kitchenObjectHolder;

    public static KitchenObject kitchenObjectSpawn(KitchenObjectSO kitchenObjectSO, IKitchenObjectHolder kitchenObjectHolder)
    {
        Transform KitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = KitchenObjectTransform.GetComponent<KitchenObject>();
        
        kitchenObject.SetKitchenObjectHolder(kitchenObjectHolder);

        return kitchenObject;
    }
    public KitchenObjectSO GetKitchenObjectsSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectHolder(IKitchenObjectHolder kitchenObjectHolder)
    {
        if (this.kitchenObjectHolder != null)
        {
            this.kitchenObjectHolder.ClearKitchenObject();
        }
        this.kitchenObjectHolder = kitchenObjectHolder;

        if (kitchenObjectHolder.HasKitchenObject()) 
        {
            Debug.LogError("kitchenObjectHolder already has a kitechen object");
        }

        kitchenObjectHolder.SetKitchenObject(this);
        transform.parent = kitchenObjectHolder.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero; 
    }
    public IKitchenObjectHolder GetKitchenObjectHolder()
    {
        return kitchenObjectHolder;
    }

    public void DestroySelf()
    {
        kitchenObjectHolder.ClearKitchenObject();
        Destroy(gameObject);
    }
    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }
}
