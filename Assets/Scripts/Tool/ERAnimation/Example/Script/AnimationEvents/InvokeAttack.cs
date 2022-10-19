using System.Collections.Generic;
using ERAnimation;

namespace ERAnimationExample
{
    [Category("ERAnimationExample")]
    public class InvokeAttack : ERAnimationEvent
    {
        public override List<ERAnimationArg> Args
        {
            get
            {
                return new List<ERAnimationArg>()
                {
                    CreateInstance<ERAnimationArg>().Init("Name", typeof(string), ""),
                    CreateInstance<ERAnimationArg>().Init("Multipler", typeof(float), "1"),
                    CreateInstance<ERAnimationArg>().Init("Knockback", typeof(float), "15"),
                };
            }
        }
        private AttackCheck check;

        public override void Init()
        {
            base.Init();
            foreach (AttackCheck check in linkedController.GetComponentsInChildren<AttackCheck>())
            {
                if (check.name == GetString("Name"))
                {
                    check.damageMultipler = GetFloat("Multipler");
                    check.knockback = GetFloat("Knockback");
                    this.check = check;
                }
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (check != null)
            {
                check.attacking = true;
            }
        }

        public override void OnLeave()
        {
            base.OnLeave();
            if (check != null)
            {
                check.attacking = false;
            }
        }
    }
}