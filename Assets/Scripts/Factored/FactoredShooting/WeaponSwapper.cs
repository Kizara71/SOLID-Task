using UnityEngine;
using System;

public class WeaponSwapper : MonoBehaviour
{
    // Event tells UI which slot is active
    public static event Action<int> OnWeaponSlotChanged; 

    [Header("Weapon Arsenal")]
    [SerializeField] private WeaponBaseClass[] weapons; // Array of Abstract WeaponBase! (DIP)
    
    private int currentWeaponIndex = 0;

    private void Start()
    {
        // Ensure all weapons are initially off
        foreach (var weapon in weapons)
        {
            weapon.Unequip();
        }

        // Equip the first weapon
        EquipWeapon(0);
    }

    private void Update()
    {
        HandleWeaponSwapping();
        HandleShootingInput();
    }

    private void HandleWeaponSwapping()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) EquipWeapon(2);
    }

    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length) return;
        if (weapons[index] == null) return;

        // Unequip current
        weapons[currentWeaponIndex].Unequip();
        
        // Equip new
        currentWeaponIndex = index;
        weapons[currentWeaponIndex].Equip();

        // Notify UI
        OnWeaponSlotChanged?.Invoke(currentWeaponIndex);
    }

    private void HandleShootingInput()
    {
        bool isClicked = Input.GetMouseButtonDown(0);
        bool isHeld = Input.GetMouseButton(0);

        // Liskov Substitution Principle: We don't care WHAT the weapon is. 
        // We just pass the input and trust it to do its specific job.
        weapons[currentWeaponIndex].ProcessInput(isClicked, isHeld);
    }
}