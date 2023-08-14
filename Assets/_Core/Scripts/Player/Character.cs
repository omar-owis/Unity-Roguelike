using System;
using System.Collections;
using UnityEngine;

public delegate void UpdateStatPanel(params ModifiableStat[] playerStats);

public class Character : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [Header("CharacterStats")]
    public CharacterData _characterData;

    [Space]
    BackPackObject backpack;
    public int CraftingLvl;

    [Header("InventoryObjects")]
    [SerializeField] private InventoryObject[] _inventories;


    [Space]
    [SerializeField] private GameObject _mainInventoryObj;
    [SerializeField] private GameObject _resourceInventoryObj;

    public static event UpdateStatPanel UpdateStatPanel;

    private bool ResourceInv = false;
    private bool MainInv = false;

    public CharacterData CharacterData { get { return _characterData; } }

    private void OnEnable()
    {
        _inputManager.inventoryEvent += OnInventory;
        _inputManager.use1Event += OnUseItem1;
        _inputManager.use2Event += OnUseItem2;
        _characterData.OnStatChangeCallback += StatUpdate;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        StatUpdate();
        _resourceInventoryObj.SetActive(false);
        _mainInventoryObj.SetActive(false);
    }

    private void OnDisable()
    {
        _inputManager.inventoryEvent -= OnInventory;
        _inputManager.use1Event -= OnUseItem1;
        _inputManager.use2Event -= OnUseItem2;
        _characterData.OnStatChangeCallback -= StatUpdate;
    }


    // pick up handling
    private void OnTriggerEnter(Collider other)
    {
        GroundItem groundItem = other.GetComponent<GroundItem>();

        if(groundItem != null)
        {
            switch (groundItem.item.type)
            {
                case ItemType.BackPack:
                    int oldInvSize = _inventories[0].inventory.Slots.Length;
                    BackPackObject oldBackpack = backpack;
                    backpack = (BackPackObject)groundItem.item;

                    for (int i = _inventories[0].DefaultInventorySize; i < _inventories[0].inventory.Slots.Length; i++)
                    {
                        oldBackpack.Inventory.Slots[i - _inventories[0].DefaultInventorySize].UpdateSlot(_inventories[0].inventory.Slots[i]);
                    }

                    if (oldBackpack != null) other.GetComponent<GroundItem>().SetItem(oldBackpack);
                    else Destroy(other.gameObject);

                    UpdateBackPack();

                    int updateUI = (_inventories[0].inventory.Slots.Length - oldInvSize) / _inventories[0].DefaultInventorySize;  // (new inv size - old inv size)

                    UpdateInventoryUISize(updateUI);
                    break;
                default:
                    foreach(InventoryObject inventory in _inventories)
                    {
                        if(inventory.AddItem(new Item(groundItem.item), groundItem.amount))
                        {
                            if (groundItem.item.type == ItemType.Trinket)
                            {
                                groundItem.item.EnableEffects(_characterData);
                            }
                            Destroy(other.gameObject);
                            break;
                        }
                    }
                    break;
            }
        }
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Slash))
        {
            Debug.Log("Player Took Dmg");
            _characterData.Damage(0.5f);
        }
    }

    private void OnApplicationQuit()
    {
        foreach(InventoryObject inventory in _inventories)
        {
            inventory.Clear();
        }
        _characterData.ResetData();
    }

    void OnInventory()
    {
        MainInventoryButton();
        if (MainInv) _inputManager.DisableCharacter();
        else _inputManager.EnableCharacter();
    }

    void OnUseItem1()
    {
        if (_inventories[1].SlotHasItem(0))
        {
            _inventories[1].GetSlots[0].ItemObject.EnableEffects(_characterData);
            _inventories[1].GetSlots[0].RemoveAmount(1);
        }
    }

    void OnUseItem2()
    {
        if (_inventories[1].SlotHasItem(1))
        {
            _inventories[1].GetSlots[1].ItemObject.EnableEffects(_characterData);
            _inventories[1].GetSlots[1].RemoveAmount(1);
        }
    }

    public void ResourceInvButton()
    {
        ResourceInv = !ResourceInv;
        _resourceInventoryObj.SetActive(ResourceInv);
    }

    public void MainInventoryButton()
    {
        MainInv = !MainInv;
        ResourceInv = false;
        Cursor.visible = MainInv;
        if (Cursor.visible) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;

        _mainInventoryObj.SetActive(MainInv);
        _resourceInventoryObj.SetActive(false);
    }

    public void UpdateInventoryUISize(int rowUpdate)
    {
        _mainInventoryObj.GetComponent<RectTransform>().sizeDelta
            = new Vector2(_mainInventoryObj.GetComponent<RectTransform>().sizeDelta.x,
            _mainInventoryObj.GetComponent<RectTransform>().sizeDelta.y + (100 * rowUpdate));

        _resourceInventoryObj.GetComponent<RectTransform>().position
            = new Vector3(_resourceInventoryObj.GetComponent<RectTransform>().position.x,
            _resourceInventoryObj.GetComponent<RectTransform>().position.y - (100 * rowUpdate), 0);
    }

    public void UpdateBackPack()
    {
        _inventories[0].UpdateCapacity(_inventories[0].DefaultInventorySize);
        _inventories[0].AddInventorySlots(backpack.Inventory);
    }

    public void StatUpdate()
    {
        UpdateStatPanel?.Invoke(_characterData.Power, _characterData.Speed, _characterData.Luck, _characterData.Defence, _characterData.CooldownReduction);
    }
}
