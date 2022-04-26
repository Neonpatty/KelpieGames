using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioSource _foleySourceMain;
    

    void Awake()
    {
        Instance = this;

        _foleySourceMain.volume = 1;
    }

    public async void PlayerEntersNewArea(AudioClip newClip)
    {
        if (_foleySourceMain.clip == newClip) return;

        await _foleySourceMain.DecreaseVolumeToZero();


        _foleySourceMain.Stop();
        _foleySourceMain.PlayClip(newClip);

        await _foleySourceMain.IncreaseVolumeToOne();
    }

    

}

public enum AudioType
{
    Foley = 0,
    Ambience = 1,
}
