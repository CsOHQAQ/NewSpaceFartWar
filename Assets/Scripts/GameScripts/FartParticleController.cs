using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FartParticleController : MonoBehaviour
{
    public float fartLastingTime = 5f;
    public float fartDMGSec = 4f;

    private ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> insideP=new List<ParticleSystem.Particle>();
        ParticleSystem.ColliderData collider=new ParticleSystem.ColliderData();
        int numInside= particle.GetTriggerParticles(ParticleSystemTriggerEventType.Inside,insideP,out collider);
        for(int i=0;i<numInside;i++)
        {
            ParticleSystem.Particle p = insideP[i];
            if (collider.GetColliderCount(i)>0)
            {
                for(int j=0;j<collider.GetColliderCount(i);j++)
                {
                    PlayerController player= collider.GetCollider(i, j).gameObject.GetComponent<PlayerController>();
                    if (player!= null) ;
                    {
                        player.Hurt(Time.deltaTime*4f);
                    }
                }
            }
        }
    }

    public void LightEmission()
    {
        particle.Play();
    }
    public void EndLightEmission()
    {
        particle.Stop();
    }
    public void HeavyEmission()
    {
        particle.Emit(10);
    }

}

