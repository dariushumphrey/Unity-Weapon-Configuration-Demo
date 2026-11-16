using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegripScript : MonoBehaviour
{
    private WeaponAnchorScript anchor;
    private float hipZStart, aimZStart;
    private float hipZEnd, aimZEnd;
    private float recoilReducePercent = 70f;
    private float hipRecoilZNew, aimRecoilZNew;
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

            Debug.Log(recoilDistance + " | " + recoilRemoval + " | " + recoilReduction);

            //recoilReducePercent /= 100;
            //recoilReducePercent *= anchor.hipRecoilPosition.z;
            hipRecoilZNew = hipZStart - recoilReduction;

            recoilDistance = aimZStart - anchor.aimRecoilPosition.z;
            recoilRemoval = recoilDistance * (recoilReducePercent / 100f);
            recoilReduction = recoilDistance - recoilRemoval;

            //recoilReducePercent /= 100;
            //recoilReducePercent *= anchor.aimRecoilPosition.z;
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
}
