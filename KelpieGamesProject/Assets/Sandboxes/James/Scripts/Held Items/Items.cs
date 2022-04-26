using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Items : MonoBehaviour
{
    public virtual void UseAbility(Camera cam, RawImage camImage, JamesNamespace.ItemHandler itemHandler)
    {

    }

    public virtual void StopAbility(Camera cam, RawImage camImage, JamesNamespace.ItemHandler itemHandler)
    {

    }
}
