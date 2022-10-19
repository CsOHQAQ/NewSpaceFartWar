using System;
using System.Collections.Generic;
using UnityEngine;

namespace ERAnimationExample
{
    public class Entity : MonoBehaviour
    {
        [Serializable]
        public class StatusIdentifier
        {
            [SerializeField]
            private List<string> identifier;

            public StatusIdentifier()
            {
                identifier = new List<string>();
            }

            public void AddIdentifier(string item)
            {
                if (!identifier.Contains(item))
                {
                    identifier.Add(item);
                }
            }

            public void RemoveIdentifier(string item)
            {
                if (identifier.Contains(item))
                {
                    identifier.Remove(item);
                }
            }

            public void Reset()
            {
                identifier.Clear();
            }

            public bool HasIdentifier(string item)
            {
                return identifier.Contains(item);
            }
        }

        public readonly StatusIdentifier statusIdentifier = new StatusIdentifier();

        public const float error = 1f;

        public float hp;
        public float attack;

        public float speed;
        public float jumpSpeed;
        public float acceleration;
        public float resistacne;

        public bool isMoving;
        public bool isFacingRight;

        private Rigidbody2D _body;
        public Rigidbody2D body
        {
            get
            {
                if (_body == null)
                {
                    _body = GetComponent<Rigidbody2D>();
                }
                return _body;
            }
        }

        public void FixedUpdate()
        {
            if (isMoving)
            {
                if (Mathf.Abs(body.velocity.x) > speed)
                {
                    body.AddForce(new Vector2((body.velocity.x > 0 ? -1 : 1) * resistacne, 0));
                }
                else
                {
                    body.AddForce(new Vector2((isFacingRight ? 1 : -1) * acceleration, 0));
                }
            }
            else
            {
                if (Mathf.Abs(body.velocity.x) > error)
                {
                    body.AddForce(new Vector2((body.velocity.x > 0 ? -1 : 1) * resistacne, 0));
                }
                else
                {
                    body.velocity = new Vector2(0, body.velocity.y);
                }
            }
        }

        public void Die()
        {

        }

        public static void Damage(Entity attacker, Entity victim, AttackCheck attackCheck)
        {
            if (!victim.statusIdentifier.HasIdentifier("Dodge"))
            {
                victim.hp -= attacker.attack * attackCheck.damageMultipler;
                victim.body.velocity += (attacker.transform.position.x > victim.transform.position.x ? -1 : 1) * new Vector2(attackCheck.knockback, 0);
                CameraManager.Instance.Shake();
                if (victim.hp <= 0)
                {
                    victim.hp = 0;
                    victim.Die();
                }
                victim.GetComponent<Animator>().SetTrigger("Damage");
            }
            else
            {
                Debug.Log("闪避！");
            }
        }
    }
}