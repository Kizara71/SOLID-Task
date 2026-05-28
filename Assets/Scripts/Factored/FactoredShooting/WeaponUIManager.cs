using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUIManager : MonoBehaviour
{
    [Header("UI Outlines")]
    [SerializeField] private Outline[] weaponOutlines; // Array matches the 1, 2, 3 slots

    [Header("Ammo Text")]
    [SerializeField] private TextMeshProUGUI centralAmmoText;

    private void OnEnable()
    {
        // Subscribe to events
        WeaponSwapper.OnWeaponSlotChanged += UpdateOutlines;
        WeaponBaseClass.OnAmmoDisplayChanged += UpdateAmmoText;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        WeaponSwapper.OnWeaponSlotChanged -= UpdateOutlines;
        WeaponBaseClass.OnAmmoDisplayChanged -= UpdateAmmoText;
    }

    private void UpdateOutlines(int activeIndex)
    {
        for (int i = 0; i < weaponOutlines.Length; i++)
        {
            if (weaponOutlines[i] != null)
            {
                // Only the outline matching the activeIndex is true
                weaponOutlines[i].enabled = (i == activeIndex);
            }
        }
    }

    private void UpdateAmmoText(string newText)
    {
        if (centralAmmoText != null)
        {
            centralAmmoText.text = newText;
        }
    }
}
