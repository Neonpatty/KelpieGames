using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableItem", menuName = "ScriptableItem")]
public class ScriptableItem : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public Items Script;
}
