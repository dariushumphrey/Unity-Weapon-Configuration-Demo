using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScopeScript : MonoBehaviour
{
    private WeaponAnchorScript anchor;
    private float startingMinZoom;
    private float newMinZoom = 15f;
    private float startingZoomSpeed;
    private float newZoomSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
