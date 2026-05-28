using UnityEngine;

public class Rifle : WeaponBaseClass, IReloadable
{
    [Header("Rifle Specs")]
    [SerializeField] private int maxAmmo = 1;
    [SerializeField] private float reloadDuration = 2.5f;
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
        IsReloading = false;
        UpdateUI($"{currentAmmo} / {maxAmmo}");
    }

    public override void ProcessInput(bool isMouseDown, bool isMouseHeld)
    {
        if (isMouseDown && !IsReloading && currentAmmo > 0)
        {
            Fire();
            Reload(); // Auto-reload after single shot
        }
    }

    private void Fire()
    {
        currentAmmo--;
        if (muzzleFlash != null) muzzleFlash.Play();
        if (audioSource != null && shotSound != null) audioSource.PlayOneShot(shotSound);

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            AttemptDamage(hit, damage);
        }
    }

    public void Reload()
    {
        IsReloading = true;
        reloadTimer = reloadDuration;
    }
}