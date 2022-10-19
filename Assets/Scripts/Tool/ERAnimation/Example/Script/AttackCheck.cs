using System.Collections.Generic;
using UnityEngine;

namespace ERAnimationExample
{
    public class AttackCheck : MonoBehaviour
    {
        private HashSet<Entity> detectedEntity = new HashSet<Entity>();
        private HashSet<Entity> attackedEntity = new HashSet<Entity>();
        private Entity parent;

        public float damageMultipler;
        public float knockback;
        public bool attacking;

        private void Awake()
        {
            parent = GetComponentInParent<Entity>();
        }

        private void Update()
        {
            if (attacking)
            {
                foreach (Entity entity in detectedEntity)
                {
                    if (!attackedEntity.Contains(entity))
                    {
                        attackedEntity.Add(entity);
                        Entity.Damage(parent, entity, this);
                    }
                }
            }
            else if (attackedEntity.Count > 0)
            {
                attackedEntity.Clear();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Entity entity = collision.gameObject.GetComponentInParent<Entity>();
            if (entity != null)
            {
                detectedEntity.Add(entity);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Entity entity = collision.gameObject.GetComponentInParent<Entity>();
            if (entity != null && detectedEntity.Contains(entity))
            {
                detectedEntity.Remove(entity);
            }
        }
    }
}
