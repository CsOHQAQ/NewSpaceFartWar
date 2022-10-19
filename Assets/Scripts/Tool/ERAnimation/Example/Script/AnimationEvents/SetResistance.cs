using System.Collections.Generic;
using ERAnimation;

namespace ERAnimationExample
{
    [Category("ERAnimationExample")]
    public class SetResistance : ERAnimationEvent
    {
        public override List<ERAnimationArg> Args
        {
            get
            {
                return new List<ERAnimationArg>()
                {
                    CreateInstance<ERAnimationArg>().Init("Resistance", typeof(float), ""),
                };
            }
        }

        private Entity entity;
        private float defaultRes;

        public override void Init()
        {
            base.Init();
            entity = linkedController.GetComponent<Entity>();
            defaultRes = entity.resistacne;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            entity.resistacne = GetFloat("Resistance");
        }

        public override void OnLeave()
        {
            base.OnEnter();
            entity.resistacne = defaultRes;
        }
    }
}