using System.Collections.Generic;
using UnityEngine;
using ERAnimation;

namespace ERAnimationExample
{
    [Category("ERAnimationExample")]
    public class SetVelocityX : ERAnimationEvent
    {
        public override List<ERAnimationArg> Args
        {
            get
            {
                return new List<ERAnimationArg>()
                {
                    CreateInstance<ERAnimationArg>().Init("Speed", typeof(float), ""),
                };
            }
        }
        private Entity entity;

        public override void Init()
        {
            base.Init();
            entity = linkedController.GetComponent<Entity>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            entity.body.velocity = new Vector2((entity.isFacingRight ? 1 : -1) * GetFloat("Speed"), 0);
        }
    }
}