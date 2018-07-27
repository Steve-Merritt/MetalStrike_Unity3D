using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

    public AudioClip audioClip_TankExplode;

	public void PlayTankExplodeAtLocation(Vector3 location)
    {
        AudioSource.PlayClipAtPoint(audioClip_TankExplode, location);
    }
}
