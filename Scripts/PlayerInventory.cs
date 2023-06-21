using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;
  [SerializeField]  public GameObject[] potionEffects;
    public Attribute[] attributes;
    public float timer;

    // public MouseItem mouseItem = new MouseItem();
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        var item = collision.GetComponent<GroundItem>();
        if (item)
        {
            ItemInst _item = new ItemInst(item.item);
            if (inventory.AddItem(_item, 1))
            {
                Destroy(collision.gameObject);
            }
            
           
        }
    }

    private void Start()
    {
        timer = 0f;
        inventory.Load();
        equipment.Load();
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }
        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }
    }
  

    private void Update()
    {

        if (timer > 0.1f)
        {

        }
        else
        {
            timer += 1 * Time.unscaledTime;
            inventory.Load();
            equipment.Load();
        }
        if (Input.GetKeyDown(KeyCode.G)){
            inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            inventory.Load();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
           
            PlayerPrefs.SetInt("AttackProgress", 0);
            PlayerPrefs.SetInt("DefenceProgress", 0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ItemInst temp;
            for (int i = 0; i < inventory.GetSlots.Length; i++)
            {
                InventorySlot temp1 = inventory.GetSlots[i];
                temp = temp1.item;
                if (temp != null)
                {
                    if (temp._type == ItemType.HealthPotion && inventory.GetSlots[i].amount > 0 && RealMovement.Instance.health < RealMovement.Instance.maxHealth)
                    {
                        inventory.GetSlots[i].amount--;
                        RealMovement.Instance.health += 40;
                        if (potionEffects[0] != null)
                            Instantiate(potionEffects[0], transform);
                        break;
                    }
                }
                else
                {
                    Debug.Log("Issue");
                }

            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ItemInst temp;
            for (int i = 0; i < inventory.GetSlots.Length; i++)
            {
                InventorySlot temp1 = inventory.GetSlots[i];
                temp = temp1.item;
                if (temp != null)
                {
                    if (temp._type == ItemType.StaminaPotion && inventory.GetSlots[i].amount > 0 && RealMovement.Instance.stamina < RealMovement.Instance.maxStamina)
                    {
                        inventory.GetSlots[i].amount--;
                        RealMovement.Instance.stamina += 40;
                        if (potionEffects[1] != null)
                            Instantiate(potionEffects[1], transform);
                        break;
                    }
                }
                else
                {
                    Debug.Log("Issue");
                }

            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ItemInst temp;
            for (int i = 0; i < inventory.GetSlots.Length; i++)
            {
                InventorySlot temp1 = inventory.GetSlots[i];
                temp = temp1.item;
                if (temp != null)
                {
                    if (temp._type == ItemType.ManaPotion && inventory.GetSlots[i].amount > 0 && RealMovement.Instance.mana < RealMovement.Instance.maxMana)
                    {
                        inventory.GetSlots[i].amount--;
                        RealMovement.Instance.mana += 40;
                        if (potionEffects[2] != null)
                            Instantiate(potionEffects[2], transform);
                        break;
                    }
                }
                else
                {
                    Debug.Log("Issue");
                }

            }

        }
    }
    private void OnApplicationQuit()
    {
        
        inventory.Clear();
       equipment.Clear();
    }

    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
        switch (attribute.type)
        {
            case Attributes.Health:
                RealMovement.Instance.maxHealth = RealMovement.Instance.baseHealth+ attribute.value.ModifiedValue;
                break;
            case Attributes.Mana:
                RealMovement.Instance.maxMana = RealMovement.Instance.baseMana + attribute.value.ModifiedValue;
                break;
            case Attributes.Attack:
                RealMovement.Instance.dmg=RealMovement.Instance.baseDamage + attribute.value.ModifiedValue;
                break;
            case Attributes.Speed:
                RealMovement.Instance.sprintSpeed = RealMovement.Instance.baseSpeed + attribute.value.ModifiedValue;
                break;
            case Attributes.Stamina:
                RealMovement.Instance.maxStamina = RealMovement.Instance.baseStamina + attribute.value.ModifiedValue;
                break;

        }
    }


    public void OnBeforeSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
                    }
                }

                break;
            
            default:
                break;
        }
    }
    public void OnAfterSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            attributes[j].value.AddModifier(_slot.item.buffs[i]);
                    }
                }

                break;
            
            default:
                break;
        }
    }
}

[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public PlayerInventory parent;
    public Attributes type;
    public ModifiableInt value;

    public void SetParent(PlayerInventory _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttributeModified);
    }
    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}
