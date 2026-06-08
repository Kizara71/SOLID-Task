using UnityEngine;

// Concrete Class for a pistol weapon
public class Pistol : WeaponBaseClass, IReloadable
{
    [Header("Pistol Specs")]
    [SerializeField] private int maxAmmo = 6;
    [SerializeField] private float reloadDuration = 1.0f;
    [SerializeField] private float damage = 25f;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shotSound;

    private int currentAmmo;
    private float reloadTimer = 0f;
    public bool IsReloading { get; private set; } = false;

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        if (IsReloading)
        {
            reloadTimer -= Time.deltaTime;
            UpdateUI("RELOADING...");

            if (reloadTimer <= 0f)
            {
                currentAmmo = maxAmmo;
                IsReloading = false;
                UpdateUI($"{currentAmmo} / {maxAmmo}");
            }
        }
    }

    public override void Equip()
    {
        base.Equip();
        IsReloading = false; // Cancel reload on swap
        UpdateUI($"{currentAmmo} / {maxAmmo}");
    }

    public override void ProcessInput(bool isMouseDown, bool isMouseHeld)
    {
        if (isMouseDown && !IsReloading)
        {
            if (currentAmmo > 0)
            {
                Fire();
            }
            else
            {
                Reload();
            }
        }
    }

    private void Fire()
    {
        currentAmmo--;
        if (muzzleFlash != null) muzzleFlash.Play();
        if (audioSource != null && shotSound != null) audioSource.PlayOneShot(shotSound);

        UpdateUI($"{currentAmmo} / {maxAmmo}");

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            AttemptDamage(hit, damage);
        }

        if (currentAmmo <= 0) Reload();
    }

    public void Reload()
    {
        IsReloading = true;
        reloadTimer = reloadDuration;
    }
}
