using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FartParticleController : MonoBehaviour
{
    private ParticleSystem particle;
    private ParticleSystem.TriggerModule triggerModule;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
                    GameObject go= collider.GetCollider(i, j).gameObject;
                    Debug.Log(go.name);
                }
            }
        }
    }
}
