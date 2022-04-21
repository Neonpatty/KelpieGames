using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioSource _foleySourceMain, _foleySourceTemp;
    

    void Awake()
    {
        Instance = this;

        _foleySourceMain.volume = 1;
    }

    public void PlayerEntersNewArea(AudioClip newClip)
    {
        _foleySourceMain.Stop();
        _foleySourceMain.PlayClip(newClip);
    }

}

public enum AudioType
{
    Foley = 0,
    Ambience = 1,
}
