using UnityEngine;
using UnityEngine.UI; // Required for accessing the Outline component
using TMPro;

public class UnfactoredShooting : MonoBehaviour
{
    public enum SelectedWeapon { Pistol, Rifle, MagicWand }
    [Header("Weapon Inventory Setup")]
    public SelectedWeapon activeWeapon = SelectedWeapon.Pistol;
    
    [Header("3D Weapon Models (On-Screen View)")]
    public GameObject pistolModel;
    public GameObject rifleModel;
    public GameObject wandModel;
    
    [Header("UI Outlines")]
    public Outline pistolSlotOutline;
    public Outline rifleSlotOutline;
    public Outline wandSlotOutline;

    [Header("Ammo UI Text")]
    public TextMeshProUGUI centralAmmoText;

    [Header("Weapon Configurations & Ammunition")]
    // Pistol
    public float pistolDamage = 20f;
    public int pistolMaxAmmo = 6;
    private int currentPistolAmmo;
    private bool isPistolReloading = false;
    private float pistolReloadTimer = 0f;
    public float pistolReloadDuration = 1.0f;

    // Rifle
    public float rifleDamage = 100f;
    public int rifleMaxAmmo = 1;
    private int currentRifleAmmo;
    private bool isRifleReloading = false;
    private float rifleReloadTimer = 0f;
    public float rifleReloadDuration = 2.5f;

    // Magic Wand
    public float wandDamagePerSecond = 5f;

    [Header("Wand Laser Components")]
    public LineRenderer wandLineRenderer;
    public Transform laserTipOrigin;

    [Header("Muzzle Flash Effects")]
    public ParticleSystem pistolMuzzleFlash;
    public ParticleSystem rifleMuzzleFlash;

    [Header("Audio Effects")]
    public AudioSource weaponAudioSource;
    public AudioClip pistolShotSound;
    public AudioClip rifleShotSound;

    void Start()
    {
        // Initialize ammunition reservoirs
        currentPistolAmmo = pistolMaxAmmo;
        currentRifleAmmo = rifleMaxAmmo;

        RefreshWeaponHUD();
    }

    void Update()
    {
        // Handle inventory weapon swaps via shortcut keys
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            activeWeapon = SelectedWeapon.Pistol;
            RefreshWeaponHUD();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            activeWeapon = SelectedWeapon.Rifle;
            RefreshWeaponHUD();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) 
        {
            activeWeapon = SelectedWeapon.MagicWand;
            RefreshWeaponHUD();
        }
        // Process pistol reload countdown if it's currently locked
        if (isPistolReloading)
        {
            pistolReloadTimer -= Time.deltaTime;
            if (activeWeapon == SelectedWeapon.Pistol)
            {
                centralAmmoText.text = "RELOADING...";
            }

            if (pistolReloadTimer <= 0f)
            {
                currentPistolAmmo = pistolMaxAmmo;
                isPistolReloading = false;
                if (activeWeapon == SelectedWeapon.Pistol) RefreshWeaponHUD();
            }
        }
        // Process rifle reload countdown if it's currently locked
        if (isRifleReloading)
        {
            rifleReloadTimer -= Time.deltaTime;
            if (activeWeapon == SelectedWeapon.Rifle)
            {
                centralAmmoText.text = "RELOADING...";
            }

            if (rifleReloadTimer <= 0f)
            {
                currentRifleAmmo = rifleMaxAmmo;
                isRifleReloading = false;
                if (activeWeapon == SelectedWeapon.Rifle) RefreshWeaponHUD();
            }
        }

        // Core Trigger Firing Processing Engine
        HandleActiveFiring();
    }

    private void HandleActiveFiring()
    {
        // Clear raycast beam visual if wand isn't currently maintained
        if (wandLineRenderer != null && activeWeapon != SelectedWeapon.MagicWand)
        {
            wandLineRenderer.enabled = false;
        }

        switch (activeWeapon)
        {
            case SelectedWeapon.Pistol:
                if (Input.GetMouseButtonDown(0))
                {
                    if (currentPistolAmmo > 0)
                    {
                        currentPistolAmmo--;
                        PerformRaycastAttack(pistolDamage);

                        // Visual and Audio Effects
                        if (pistolMuzzleFlash != null) pistolMuzzleFlash.Play();
                        if (weaponAudioSource != null && pistolShotSound != null) 
                            weaponAudioSource.PlayOneShot(pistolShotSound);
                    }
                    if (currentPistolAmmo <= 0)
                    {
                        isPistolReloading = true;
                        pistolReloadTimer = pistolReloadDuration;
                    }
                    RefreshWeaponHUD();
                }
                break;

            case SelectedWeapon.Rifle:
                if (Input.GetMouseButtonDown(0) && !isRifleReloading && currentRifleAmmo > 0)
                {
                    currentRifleAmmo--;
                    PerformRaycastAttack(rifleDamage);

                    // Visual and Audio Effects
                    if (rifleMuzzleFlash != null) rifleMuzzleFlash.Play();
                    if (weaponAudioSource != null && rifleShotSound != null) 
                        weaponAudioSource.PlayOneShot(rifleShotSound);

                    // Immediately trigger heavy reload sequence logic
                    isRifleReloading = true;
                    rifleReloadTimer = rifleReloadDuration;

                    RefreshWeaponHUD();
                }
                break;

            case SelectedWeapon.MagicWand:
                if (Input.GetMouseButton(0)) // Constant holding
                {
                    PerformContinuousLaserAttack();
                }
                else
                {
                    if (wandLineRenderer != null) wandLineRenderer.enabled = false;
                }
                break;
        }
    }

    private void PerformRaycastAttack(float damageAmount)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // TIGHT COUPLING: Directly looking for the concrete UnfactoredEnemy script component
            UnfactoredEnemy enemy = hit.transform.GetComponent<UnfactoredEnemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }
            else
            {
                Debug.Log("Hit environment object: " + hit.transform.name);
            }
        }
    }

    private void PerformContinuousLaserAttack()
    {
        if (wandLineRenderer == null) return;

        wandLineRenderer.enabled = true;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        wandLineRenderer.SetPosition(0, laserTipOrigin.position);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            wandLineRenderer.SetPosition(1, hit.point);

            // TIGHT COUPLING: Continuous tick damage directly hitting the enemy component frame-by-frame
            UnfactoredEnemy enemy = hit.transform.GetComponent<UnfactoredEnemy>();
            if (enemy != null)
            {
                float frameDamage = wandDamagePerSecond * Time.deltaTime;
                enemy.TakeDamage(frameDamage);
            }
        }
        else
        {
            wandLineRenderer.SetPosition(1, ray.origin + ray.direction * 40f);
        }
    }

    private void RefreshWeaponHUD()
    {
        // 1. Hardcoded 3D Model Toggles (Violates OCP/SRP, perfect for unfactored version!)
        if (pistolModel != null)
        {
            pistolModel.SetActive(activeWeapon == SelectedWeapon.Pistol);
        }
        if (rifleModel != null)
        {
            rifleModel.SetActive(activeWeapon == SelectedWeapon.Rifle);
        }
        if (wandModel != null)
        {
            wandModel.SetActive(activeWeapon == SelectedWeapon.MagicWand);
        }

        // 2. Tightly coupled Outline component switching logic (SRP Violation)
        if (pistolSlotOutline != null)
        {
            pistolSlotOutline.enabled = (activeWeapon == SelectedWeapon.Pistol);
        }
        if (rifleSlotOutline != null)
        {
            rifleSlotOutline.enabled = (activeWeapon == SelectedWeapon.Rifle);
        }
        if (wandSlotOutline != null)
        {
            wandSlotOutline.enabled = (activeWeapon == SelectedWeapon.MagicWand);
        }

        // 3. Monolithic Single UI string formatter processing loop (SRP / OCP Violation)
        if (centralAmmoText == null) return;

        switch (activeWeapon)
        {
            case SelectedWeapon.Pistol:
                centralAmmoText.text = currentPistolAmmo + " / " + pistolMaxAmmo;
                break;

            case SelectedWeapon.Rifle:
                if (isRifleReloading)
                {
                    centralAmmoText.text = "RELOADING...";
                }
                else
                {
                    centralAmmoText.text = currentRifleAmmo + " / " + rifleMaxAmmo;
                }
                break;

            case SelectedWeapon.MagicWand:
                centralAmmoText.text = "inf / inf";
                break;
        }
    }
}