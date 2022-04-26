using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeaGlider : Items
{
    public bool IsFlying { get; private set; } = false;

    [SerializeField] int _speedMultiplier;

    public override void UseAbility(Camera cam, RawImage camImage, JamesNamespace.ItemHandler itemHandler)
    {
        IsFlying = true;
        cam.GetComponentInParent<JamesNamespace.SwimmingController>().MultiplySpeed(_speedMultiplier);
    }

    public override void StopAbility(Camera cam, RawImage camImage, JamesNamespace.ItemHandler itemHandler)
    {
        IsFlying = false;
        cam.GetComponentInParent<JamesNamespace.SwimmingController>().DivideSpeed(_speedMultiplier);
    }

}
