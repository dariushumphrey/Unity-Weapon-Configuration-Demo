using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponAnchorScript : MonoBehaviour
{
    public GameObject weapon;
    public bool hasAttachments = false;
    public bool toggleConfigure = false;
    public bool aiming = false;
    public float actionSpeed = 3f;
    public float adsSpeed = 50f;
    public Vector3 restPosition = Vector3.zero;
    public Vector3 aimRestPosition = Vector3.zero;
    public Vector3 hipRecoilPosition;
    public Vector3 aimRecoilPosition;
    public Quaternion configureRotation = Quaternion.identity;
    public Text configureNotice;
    public GameObject configurePage;
    public Camera gunCam;
    public float camZoom;
    public float minZoom, maxZoom;
    public Image zoomImage;
    public Text weaponCurrentMag;
    public Sprite blankReticle;

    private float repeatTimer = 0f;
    private bool recoilToRest = false;
    private bool repeatTimeout = false;
    private bool restToConfigure = false;
    private bool configureToRest = false;
    private bool zoomEnabled = false;
    private bool switchInProgress = false;
    private Sprite weaponStartingReticle;
    [HideInInspector] public Vector3 recoilPosition;
    [HideInInspector] public bool enableScopeOverlay;
    [HideInInspector] public int weaponLayer;
    internal float fireRateInsert;
    // Start is called before the first frame update
    void Start()
    {
        if(hasAttachments)
        {
            configureNotice.text = " Press 'E' to configure " + weapon.name;
        }

        recoilPosition = hipRecoilPosition;

        camZoom = gunCam.fieldOfView;

        weaponLayer = LayerMask.NameToLayer("Weapon");
        weaponStartingReticle = weapon.GetComponent<AssaultRifleScript>().reticle.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        camZoom = Mathf.Clamp(camZoom, minZoom, maxZoom);
        gunCam.fieldOfView = camZoom;

        weaponCurrentMag.text = weapon.GetComponent<AssaultRifleScript>().currentAmmo.ToString();

        if(!zoomEnabled)
        {
            camZoom += adsSpeed * Time.deltaTime;
            if (camZoom >= maxZoom)
            {
                camZoom = maxZoom;
            }
        }

        if(hasAttachments)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(!toggleConfigure)
                {
                    if(!switchInProgress)
                    {
                        toggleConfigure = true;
                        restToConfigure = true;
                        switchInProgress = true;
                        weapon.GetComponent<AssaultRifleScript>().enabled = false;
                        weapon.GetComponent<AssaultRifleScript>().reticle.sprite = blankReticle;
                        configureNotice.text = "";
                        configurePage.SetActive(true);

                        aiming = false;
                        zoomEnabled = false;
                        recoilPosition = hipRecoilPosition;
                        if (enableScopeOverlay)
                        {
                            if (zoomImage.gameObject.activeInHierarchy)
                            {
                                zoomImage.gameObject.SetActive(false);
                                gunCam.cullingMask |= (1 << weaponLayer);
                            }
                        }
                    }
                }

                else
                {
                    if(!switchInProgress)
                    {
                        toggleConfigure = false;
                        configureToRest = true;
                        switchInProgress = true;
                        weapon.GetComponent<AssaultRifleScript>().enabled = true;
                        weapon.GetComponent<AssaultRifleScript>().reticle.sprite = weaponStartingReticle;
                        configureNotice.text = " Press 'E' to configure " + weapon.name;
                        configurePage.SetActive(false);
                    }
                }
            }
        }

        if (restToConfigure)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, configureRotation, actionSpeed * Time.deltaTime);
            if (Quaternion.Angle(transform.localRotation, configureRotation) < 0.1f)
            {
                restToConfigure = false;
                DisableSwitchTimeout();
            }
        }

        if (configureToRest)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, actionSpeed * Time.deltaTime);
            if (Quaternion.Angle(transform.localRotation, Quaternion.identity) < 0.1f)
            {
                configureToRest = false;
                DisableSwitchTimeout();
            }
        }

        if(!toggleConfigure)
        {
            if (Input.GetButton("Fire2"))
            {
                aiming = true;
                zoomEnabled = true;
                recoilPosition = aimRecoilPosition;

                camZoom -= adsSpeed * Time.deltaTime;
                if (camZoom <= minZoom)
                {
                    camZoom = minZoom;
                    if (enableScopeOverlay)
                    {
                        if (!zoomImage.gameObject.activeInHierarchy)
                        {
                            zoomImage.gameObject.SetActive(true);
                            gunCam.cullingMask &= ~(1 << weaponLayer);
                        }
                    }
                }
            }

            if (Input.GetButtonUp("Fire2"))
            {
                aiming = false;
                zoomEnabled = false;
                recoilPosition = hipRecoilPosition;
                if (enableScopeOverlay)
                {
                    if (zoomImage.gameObject.activeInHierarchy)
                    {
                        zoomImage.gameObject.SetActive(false);
                        gunCam.cullingMask |= (1 << weaponLayer);
                    }
                }
            }
        }
      
        if (aiming)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimRestPosition, (actionSpeed * Time.deltaTime));
            if (Vector3.Distance(transform.localPosition, aimRestPosition) <= 0.001f)
            {
                transform.localPosition = aimRestPosition;
            }

            if (recoilToRest)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, aimRestPosition, (actionSpeed * Time.deltaTime));
                if (Vector3.Distance(transform.localPosition, aimRestPosition) <= 0.001f)
                {
                    transform.localPosition = aimRestPosition;
                    recoilToRest = false;
                }
            }
        }

        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, restPosition, (actionSpeed * Time.deltaTime));
            if (Vector3.Distance(transform.localPosition, restPosition) <= 0.001f)
            {
                transform.localPosition = restPosition;
            }         
        }

        if (repeatTimeout)
        {
            repeatTimer += Time.deltaTime;
            if (repeatTimer >= fireRateInsert)
            {
                repeatTimer = 0f;
                repeatTimeout = false;
            }
        }              
    }

    public void SimulateRecoil()
    {
        if (!repeatTimeout)
        {
            transform.localPosition = recoilPosition;
            recoilToRest = true;
            repeatTimeout = true;
        }
    }

    public void DisableSwitchTimeout()
    {
        switchInProgress = false;
    }

    public void ToggleAttachment(GameObject attachment)
    {
        if(!attachment.gameObject.activeInHierarchy)
        {
            attachment.SetActive(true);
        }

        else
        {
            attachment.SetActive(false);
        }
    }
}
