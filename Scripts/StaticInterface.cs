using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StaticInterface : UserInterface
{
    public GameObject[] slots;
    [Header("Inventory Stats")]
    [SerializeField] Text HealthLabel;
    [SerializeField] Text StaminaLabel;
    [SerializeField] Text SpeedLabel;
    [SerializeField] Text ManaLabel;
    [SerializeField] Text AttackLabel;

    public override void CreateSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var obj = slots[i];


            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            inventory.GetSlots[i].slotDisplay = obj;

            slotsOnInterface.Add(obj, inventory.GetSlots[i]);

        }
    }

    private void Update()
    {
        HealthLabel.text = "HP: " + RealMovement.Instance.maxHealth;
        StaminaLabel.text = "SP: " + RealMovement.Instance.maxStamina;
        SpeedLabel.text = "Speed: " + RealMovement.Instance.maxSpeed;
        ManaLabel.text = "Mana: " + RealMovement.Instance.maxMana;
        AttackLabel.text = "ATK: " + RealMovement.Instance.dmg;
    }
}
