using UnityEngine;
using System;

public abstract class WeaponBaseClass : MonoBehaviour
{
    // C# Event to tell the UI to update without directly referencing UI components
    public static event Action<string> OnAmmoDisplayChanged;

    [Header("Base Weapon Settings")]
    [SerializeField] private GameObject weaponModel;
    [SerializeField] protected float damage;

    // Virtual methods allow standard behavior that subclasses can override if needed
    public virtual void Equip()
    {
        if (weaponModel != null) weaponModel.SetActive(true);
    }

    public virtual void Unequip()
    {
        if (weaponModel != null) weaponModel.SetActive(false);
    }

    // Abstract method: Every subclass MUST implement this, guaranteeing LSP.
    public abstract void ProcessInput(bool isMouseDown, bool isMouseHeld);

    // Helper method for subclasses to push text to the UI
    protected void UpdateUI(string text)
    {
        OnAmmoDisplayChanged?.Invoke(text);
    }

    // Shared raycast logic relying on abstractions (DIP)
    protected void AttemptDamage(RaycastHit hit, float damageAmount)
    {
        // Relying on the IDamageable ABSTRACTION, not a concrete Enemy class
        IDamageable target = hit.transform.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damageAmount);
        }
    }
}
