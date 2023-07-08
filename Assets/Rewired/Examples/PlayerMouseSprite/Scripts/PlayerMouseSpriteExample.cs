


namespace Rewired.Demos {
    using UnityEngine;

    [AddComponentMenu("")]
    public class PlayerMouseSpriteExample : MonoBehaviour {

#if UNITY_4_6_PLUS
        [Tooltip("The Player that will control the mouse")]
#endif
        /// <summary>
        /// 控制鼠标的玩家id
        /// </summary>
        public int playerId = 0;

#if UNITY_4_6_PLUS
        [Tooltip("The Rewired Action used for the mouse horizontal axis.")]
#endif
        /// <summary>
        /// 鼠标横轴
        /// </summary>
        public string horizontalAction = "MouseX";

#if UNITY_4_6_PLUS
        [Tooltip("The Rewired Action used for the mouse vertical axis.")]
#endif
        /// <summary>
        /// 鼠标纵轴输入
        /// </summary>
        public string verticalAction = "MouseY";

#if UNITY_4_6_PLUS
        [Tooltip("The Rewired Action used for the mouse wheel axis.")]
#endif
        /// <summary>
        /// 滚轮输入
        /// </summary>
        public string wheelAction = "MouseWheel";

#if UNITY_4_6_PLUS
        [Tooltip("The Rewired Action used for the mouse left button.")]
#endif
        /// <summary>
        /// 左键
        /// </summary>
        public string leftButtonAction = "MouseLeftButton";

#if UNITY_4_6_PLUS
        [Tooltip("The Rewired Action used for the mouse right button.")]
#endif
        /// <summary>
        /// 右键
        /// </summary>
        public string rightButtonAction = "MouseRightButton";

#if UNITY_4_6_PLUS
        [Tooltip("The Rewired Action used for the mouse middle button.")]
#endif
        /// <summary>
        /// 右键
        /// </summary>
        public string middleButtonAction = "MouseMiddleButton";

#if UNITY_4_6_PLUS
        [Tooltip("The distance from the camera that the pointer will be drawn.")]
#endif
        /// <summary>
        /// 距离摄像机绘制的距离
        /// </summary>
        public float distanceFromCamera = 1f;

#if UNITY_4_6_PLUS
        [Tooltip("The scale of the sprite pointer.")]
#endif
        /// <summary>
        /// 指针的尺寸
        /// </summary>
        public float spriteScale = 0.05f;

#if UNITY_4_6_PLUS
        [Tooltip("The pointer prefab.")]
#endif
        /// <summary>
        /// 指针预制体
        /// </summary>
        public GameObject pointerPrefab;

#if UNITY_4_6_PLUS
        [Tooltip("The click effect prefab.")]
#endif
        /// <summary>
        /// 点击特效预制体
        /// </summary>
        public GameObject clickEffectPrefab;

#if UNITY_4_6_PLUS
        [Tooltip("Should the hardware pointer be hidden?")]
#endif
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool hideHardwarePointer = true;
        /// <summary>
        /// 实际对象
        /// </summary>
        [System.NonSerialized]
        private GameObject pointer;
        /// <summary>
        /// 插件内部的鼠标
        /// </summary>
        [System.NonSerialized]
        private PlayerMouse mouse;

        void Awake() {

            pointer = (GameObject)GameObject.Instantiate(pointerPrefab);
            pointer.transform.localScale = new Vector3(spriteScale, spriteScale, spriteScale);

            if(hideHardwarePointer) Cursor.visible = false; // hide the hardware pointer

            // Create the Player Mouse
            mouse = PlayerMouse.Factory.Create();

            // Set the owner
            mouse.playerId = playerId;

            // Set up Actions for each axis and button
            mouse.xAxis.actionName = horizontalAction;
            mouse.yAxis.actionName = verticalAction;
            mouse.wheel.yAxis.actionName = wheelAction;
            mouse.leftButton.actionName = leftButtonAction;
            mouse.rightButton.actionName = rightButtonAction;
            mouse.middleButton.actionName = middleButtonAction;

            // If you want to change joystick pointer speed
            mouse.pointerSpeed = 1f;

            // If you want to change the wheel to tick more often
            mouse.wheel.yAxis.repeatRate = 5;

            // If you want to set the screen position
            mouse.screenPosition = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

            // If you want to use Actions to drive the X/Y position of the mouse
            // instead of using the hardware cursor position
            // mouse.useHardwareCursorPositionForMouseInput = false;

            // Additionally you'd need to bind mouse X/Y to the X/Y Actions on the Player's Mouse Map.
            // The result of this is that the mouse pointer will no longer pop to the hardware cursor
            // position when you start using the mouse. You would also need to hide the mouse
            // pointer using Cursor.visible = false;

            // Subscribe to position changed event (or you could just poll for it)
            mouse.ScreenPositionChangedEvent += OnScreenPositionChanged;

            // Get the initial position
            OnScreenPositionChanged(mouse.screenPosition);
        }

        void Update() {
            if (!ReInput.isReady) return;
            // Use the left or right button to create an object where you clicked
            if (mouse.leftButton.justPressed) CreateClickEffect(new Color(0f, 1f, 0f, 1f)); // green for left
            if (mouse.rightButton.justPressed) CreateClickEffect(new Color(1f, 0f, 0f, 1f)); // red for right
            if(mouse.middleButton.justPressed) CreateClickEffect(new Color(1f, 1f, 0f, 1f)); // yellow for middle
        }

        void OnDestroy() {
            if (!ReInput.isReady) return;
            mouse.ScreenPositionChangedEvent -= OnScreenPositionChanged;
        }

        void CreateClickEffect(Color color) {
            GameObject go = (GameObject)GameObject.Instantiate(clickEffectPrefab);
            go.transform.localScale = new Vector3(spriteScale, spriteScale, spriteScale);
            go.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mouse.screenPosition.x, mouse.screenPosition.y, distanceFromCamera));
            go.GetComponentInChildren<SpriteRenderer>().color = color;
            Object.Destroy(go, 0.5f);
        }

        // Callback when the screen position changes
        void OnScreenPositionChanged(Vector2 position) {

            // Convert from screen space to world space
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, distanceFromCamera));

            // Move the pointer object
            pointer.transform.position = worldPos;
        }
    }
}