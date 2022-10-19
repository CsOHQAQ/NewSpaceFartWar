using ERAnimation;

namespace ERAnimationExample
{
    [Category("ERAnimationExample")]
    public class Move : ERAnimationEvent
    {
        private Entity entity;

        public override void Init()
        {
            base.Init();
            entity = linkedController.GetComponent<Entity>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            entity.isMoving = true;
        }

        public override void OnLeave()
        {
            base.OnLeave();
            entity.isMoving = false;
        }
    }
}