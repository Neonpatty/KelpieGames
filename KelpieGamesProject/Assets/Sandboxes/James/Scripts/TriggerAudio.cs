using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAudio : MonoBehaviour
{
    [SerializeField] AudioType _type;
    [SerializeField] AudioClip _clip;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (_type)
            {
                case AudioType.Foley:
                    AudioManager.Instance.PlayerEntersNewArea(_clip);
                    break;
                case AudioType.Ambience:

                    break;

            }
        }
    }
}
