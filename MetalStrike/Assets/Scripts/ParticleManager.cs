using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

    public ParticleSystem PS_TankExplode;

    public void PlayTankExplodeAtLocation(Vector3 location)
    {
        ParticleSystem ps = Instantiate(PS_TankExplode, location, Quaternion.identity) as ParticleSystem;
        Destroy(ps.gameObject, ps.main.startLifetime.constant);
    }
}
