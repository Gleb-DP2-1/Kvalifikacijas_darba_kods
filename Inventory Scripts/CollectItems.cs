using UnityEngine;

public class CollectItems : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            ItemObject itemObject = other.GetComponent<ItemObject>();
            if (itemObject != null)
            {
                Item item = itemObject.item;
                Inventory.instance.AddItem(item);
            }

            Destroy(other.gameObject);
        }
    }
}
