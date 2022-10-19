using UnityEngine;

namespace ERAnimationExample
{
    public class PlayerController : MonoBehaviour
    {
        Entity entity;
        Animator animator;

        private void Awake()
        {
            entity = GetComponent<Entity>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.A) != Input.GetKey(KeyCode.D) && !entity.statusIdentifier.HasIdentifier("DisableTurning"))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    entity.isFacingRight = false;
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    entity.isFacingRight = true;
                    transform.localScale = new Vector3(-1, 1, 1);
                }
            }

            animator.SetBool("Moving", Input.GetKey(KeyCode.A) != Input.GetKey(KeyCode.D));

            if (Input.GetKeyDown(KeyCode.J))
            {
                animator.SetTrigger("Attack");
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                animator.SetTrigger("Roll");
            }
        }

        private string _trigger = null;
        private void SetTrigger(string trigger)
        {
            if (!string.IsNullOrEmpty(_trigger))
            {
                animator.ResetTrigger(_trigger);
            }
            _trigger = trigger;
            animator.SetTrigger(_trigger);
        }
        private void LateUpdate()
        {
            if (!string.IsNullOrEmpty(_trigger))
            {
                animator.ResetTrigger(_trigger);
            }
        }
    }
}