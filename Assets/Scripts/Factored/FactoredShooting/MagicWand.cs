using UnityEngine;

public class MagicWand : WeaponBaseClass
{
    [Header("Wand Specs")]
    [SerializeField] private LineRenderer beamRenderer;
    [SerializeField] private Transform laserTipOrigin;
    // MagicWand uses 'damage' from base class as damage-per-second

    public override void Equip()
    {
        base.Equip();
        UpdateUI("inf / inf");
        if (beamRenderer != null) beamRenderer.enabled = false;
    }

    public override void Unequip()
    {
        base.Unequip();
        if (beamRenderer != null) beamRenderer.enabled = false;
    }

    public override void ProcessInput(bool isMouseDown, bool isMouseHeld)
    {
        // Wand relies on the holding state, completely ignoring mouse down
        if (isMouseHeld)
        {
            FireBeam();
        }
        else
        {
            if (beamRenderer != null) beamRenderer.enabled = false;
        }
    }

    private void FireBeam()
    {
        if (beamRenderer == null || laserTipOrigin == null) return;

        beamRenderer.enabled = true;
        beamRenderer.SetPosition(0, laserTipOrigin.position);

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            beamRenderer.SetPosition(1, hit.point);
            float frameDamage = damage * Time.deltaTime;
            AttemptDamage(hit, frameDamage);
        }
        else
        {
            beamRenderer.SetPosition(1, ray.origin + ray.direction * 40f);
        }
    }
}
