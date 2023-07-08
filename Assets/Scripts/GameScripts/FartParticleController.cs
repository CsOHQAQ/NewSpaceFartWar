using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FartParticleController : MonoBehaviour
{
    public float fartLastingTime = 5f;
    public float fartDMGSec = 4f;

    private ParticleSystem particleSys;
    // Start is called before the first frame update
    void Start()
    {
        particleSys = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> insideP=new List<ParticleSystem.Particle>();
        ParticleSystem.ColliderData collider=new ParticleSystem.ColliderData();
        int numInside= particleSys.GetTriggerParticles(ParticleSystemTriggerEventType.Inside,insideP,out collider);
        for(int i=0;i<numInside;i++)
        {
            ParticleSystem.Particle p = insideP[i];
            if (p.startLifetime - p.remainingLifetime < 0.5f || p.remainingLifetime < 0.5f)
            {
                Debug.Log("不在可伤害时间中，跳过伤害判定");
                continue;
            }
            if (collider.GetColliderCount(i)>0)
            {
                for(int j=0;j<collider.GetColliderCount(i);j++)
                {
                    PlayerController player= collider.GetCollider(i, j).gameObject.GetComponent<PlayerController>();
                    Debug.Log($"正在对玩家{player.playerIndex}造成伤害");
                    if (player!= null)
                    {
                        player.Hurt(Time.deltaTime*0.5f);
                    }
                }
            }
        }
    }

    public void LightEmission()
    {
        if(!particleSys.isEmitting)
            particleSys.Play();
    }
    public void EndLightEmission()
    {
        particleSys.Stop();
    }
    public void HeavyEmission()
    {
        particleSys.Emit(15);
    }

}

