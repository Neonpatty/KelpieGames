using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableItem", menuName = "ScriptableItem")]
public class ScriptableItem : ScriptableObject
{
    public string Name { get { return _name; } private set { _name = value; } }

    [SerializeField] string _name = null;

    public Sprite Icon { get { return _icon; } private set { _icon = value; } }

    [SerializeField] Sprite _icon = null;

    public Items Script { get { return _script; } private set { _script = value; } }

    [SerializeField] Items _script = null;

    public AudioClip Audio { get { return _audio; } private set { _audio = value; } }
    [SerializeField]
    private AudioClip _audio = null;
}
