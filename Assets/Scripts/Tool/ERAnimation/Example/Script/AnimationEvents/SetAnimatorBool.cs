using System.Collections.Generic;
using ERAnimation;
using UnityEngine;

namespace ERAnimationExample
{
    [Category("ERAnimationExample")]
    public class SetAnimatorBool : ERAnimationEvent
    {
        public override List<ERAnimationArg> Args
        {
            get
            {
                return new List<ERAnimationArg>()
                {
                    CreateInstance<ERAnimationArg>().Init("Name", typeof(string), ""),
                    CreateInstance<ERAnimationArg>().Init("IsOpened", typeof(bool), "false"),
                };
            }
        }

        bool isOpened;
        string parName;

        public override void Init()
        {
            base.Init();
            isOpened = GetBool("IsOpened");
            parName = GetString("Name");
            linkedController.Animator.SetBool(parName, isOpened);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            linkedController.Animator.SetBool(parName, !isOpened);
        }

        public override void OnLeave()
        {
            base.OnLeave();
            linkedController.Animator.SetBool(parName, isOpened);
        }
    }
}