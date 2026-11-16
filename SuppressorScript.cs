using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuppressorScript : MonoBehaviour
{
    public ParticleSystem suppressorMuzzleFlash;
    private ParticleSystem baseMuzzleFlash;
    private ARFireExpandScript reticleInfluence;
    private AssaultRifleScript rifle;
    private float baseExpandRate;
    private float expandRatePercent = 35f;
    private float newExpandRate;
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
}
