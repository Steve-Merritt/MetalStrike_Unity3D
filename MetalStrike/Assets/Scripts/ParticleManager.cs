using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

    public ParticleSystem PS_TankExplode;
    public ParticleSystem PS_FireHit;

    public void PlayTankExplodeAtLocation(Vector3 location)
    {
        ParticleSystem ps = Instantiate(PS_TankExplode, location, Quaternion.identity) as ParticleSystem;
        Destroy(ps.gameObject, ps.main.startLifetime.constant);
    }

    public void PlayFireHitAtLocation(Vector3 location)
    {
        if (PS_FireHit == null) return;

        //Vector3 position = transform.position + Vector3.right * -0.4f + Vector3.up * 0.4f; TODO: adjust hit effect
        ParticleSystem ps = Instantiate(PS_FireHit, location, Quaternion.identity) as ParticleSystem;
        Destroy(ps.gameObject, ps.main.startLifetime.constant);
    }
}
