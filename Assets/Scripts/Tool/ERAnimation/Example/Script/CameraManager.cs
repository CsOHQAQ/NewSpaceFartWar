using UnityEngine;

namespace ERAnimationExample
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;
        public float amp;
        public float time;

        public float counter;

        private void Awake()
        {
            Instance = this;
            Physics2D.IgnoreLayerCollision(0, 0);
            Physics2D.IgnoreLayerCollision(1, 1);
        }

        public void Shake()
        {
            counter = time;
        }

        private void Update()
        {
            if (counter > 0)
            {
                counter -= Time.unscaledDeltaTime;
                transform.position = (counter / time) * amp * Random.insideUnitCircle.normalized;
                transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            }
            else
            {
                transform.position = new Vector3(0, 0, -10);
            }
        }
    }
}