using System.Collections.Generic;
using ERAnimation;

namespace ERAnimationExample
{
    [Category("ERAnimationExample")]
    public class SetIdentifier : ERAnimationEvent
    {
        public override List<ERAnimationArg> Args
        {
            get
            {
                return new List<ERAnimationArg>()
                {
                    CreateInstance<ERAnimationArg>().Init("Identifier", typeof(string), ""),
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
            entity.statusIdentifier.AddIdentifier(GetString("Identifier"));
        }

        public override void OnLeave()
        {
            base.OnLeave();
            entity.statusIdentifier.RemoveIdentifier(GetString("Identifier"));
        }
    }
}