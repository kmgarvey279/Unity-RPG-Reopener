using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemInteractionManager : MonoBehaviour
{
    public static ItemInteractionManager instance;

    [SerializeField] private ActionPopup itemPopup;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static void GetItem(ItemContainer itemContainer)
    {
        if (SaveManager.Instance.LoadedData.PlayerData.InventoryData.PickupCheck(itemContainer.ItemPickup.Item))
        {
            SaveManager.Instance.LoadedData.PlayerData.OnPickupItem(SceneManager.GetActiveScene().name, itemContainer.ItemInstanceID.Guid, itemContainer.ItemPickup.Item);
            itemContainer.DeactivateItem();

            instance.itemPopup.Display(itemContainer.ItemPickup.Item.ItemName, itemContainer.ItemPickup.Item.ItemIcon);
            instance.StartCoroutine(ResolveGetItemCo());
        }
        else
        {
            Debug.Log("Unable to pick up item!");
        }
    }

    public static IEnumerator ResolveGetItemCo()
    {
        yield return new WaitForSeconds(1f);
        instance.itemPopup.Hide();
    }
}
