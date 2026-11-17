# Unity Weapon Configuration - Demo
https://github.com/user-attachments/assets/cb8e7faa-df9e-4f60-b512-5c907d689163

# Explanation
The configuration page holds a set of attachments to toggle for a weapon and a short statement on how the weapon's handling will change when active. Each attachment has a script that alters a specific weapon metric, becoming active on first use and when re-applied to a weapon. When an attachment is removed, the affected attributes are restored to normal values.

## Suppressor
The Suppressor reduces the weapon's shot spread by **65%**, packing the shots fired more tightly. The change is reflected in the reticle's expansion rate, expanding more slowly while equipped.
 * The Suppressor also replaces the weapon's starting muzzle flash with barrel smoke.
```csharp
private void OnEnable()
{
  if(!reticleInfluence)
  {
    reticleInfluence = FindObjectOfType<ARFireExpandScript>();
    baseExpandRate = reticleInfluence.expandRate;

    expandRatePercent /= 100;
    expandRatePercent *= baseExpandRate;
    newExpandRate = expandRatePercent;
            
    reticleInfluence.expandRate = newExpandRate;
  }
        
  else
  {
    reticleInfluence.expandRate = newExpandRate;
  }

  if(!rifle)
  {
    rifle = GetComponentInParent<AssaultRifleScript>();

    if(!baseMuzzleFlash)
    {
      baseMuzzleFlash = rifle.muzzleFlash;
    }

    rifle.muzzleFlash = suppressorMuzzleFlash;
  }

  else
  {
    rifle.muzzleFlash = suppressorMuzzleFlash;
  }
}

private void OnDisable()
{
  reticleInfluence.expandRate = baseExpandRate;
  rifle.muzzleFlash = baseMuzzleFlash;
}
```
## Foregrip
The Foregrip shortens the weapon's recoil distance by **70%**. The change can be observed in the weapon's recovery speed as it returns to rest during the aim-down-sights and hip-fire states.
```csharp
private void OnEnable()
{
  if(!anchor)
  {
    anchor = FindObjectOfType<WeaponAnchorScript>();
    hipZStart = anchor.restPosition.z;
    hipZEnd = anchor.hipRecoilPosition.z;
    aimZStart = anchor.aimRestPosition.z;
    aimZEnd = anchor.aimRecoilPosition.z;

    float recoilDistance = hipZStart - anchor.hipRecoilPosition.z;
    float recoilRemoval = recoilDistance * (recoilReducePercent / 100f);
    float recoilReduction = recoilDistance - recoilRemoval;

    hipRecoilZNew = hipZStart - recoilReduction;

    recoilDistance = aimZStart - anchor.aimRecoilPosition.z;
    recoilRemoval = recoilDistance * (recoilReducePercent / 100f);
    recoilReduction = recoilDistance - recoilRemoval;

    aimRecoilZNew = aimZStart - recoilReduction;

    anchor.hipRecoilPosition.z = hipRecoilZNew;
    anchor.aimRecoilPosition.z = aimRecoilZNew;

    anchor.recoilPosition = anchor.hipRecoilPosition;

  }

  else
  {
    anchor.hipRecoilPosition.z = hipRecoilZNew;
    anchor.aimRecoilPosition.z = aimRecoilZNew;

    anchor.recoilPosition = anchor.hipRecoilPosition;

  }
}

private void OnDisable()
{
  anchor.hipRecoilPosition.z = hipZEnd;
  anchor.aimRecoilPosition.z = aimZEnd;

  anchor.recoilPosition = anchor.hipRecoilPosition;
}
```
## Scope
The Scope enables the scope-in feature and narrows the camera's Field of View (FOV). When zooming in, the change is telegraphed by the scope-in screen overlay.
 * The scope also doubles the aim-down-sights (ADS) speed of the weapon, improving the weapon's aiming responsiveness.
 * When zoomed in, the weapon mesh is culled from the camera.
```csharp
private void OnEnable()
{
  if (!anchor)
  {
    anchor = FindObjectOfType<WeaponAnchorScript>();
    anchor.enableScopeOverlay = true;
    startingMinZoom = anchor.minZoom;
    anchor.minZoom = newMinZoom;

    startingZoomSpeed = anchor.adsSpeed;
    newZoomSpeed = startingZoomSpeed * 2;
    anchor.adsSpeed = newZoomSpeed;
  }

  else
  {
    anchor.enableScopeOverlay = true;
    anchor.minZoom = newMinZoom;
    anchor.adsSpeed = newZoomSpeed;
  }
}

private void OnDisable()
{
  anchor.enableScopeOverlay = false;
  anchor.minZoom = startingMinZoom;
  anchor.adsSpeed = startingZoomSpeed;
}
```
