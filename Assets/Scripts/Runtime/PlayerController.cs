using UnityEngine;
using UnityEngine.InputSystem;

namespace HarvestLeague
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 4.5f;
        [SerializeField] private float carrySpeedMultiplier = 0.9f;
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private PlayerCarry playerCarry;

        public Vector2 FacingDirection { get; private set; } = Vector2.down;

        private Vector2 moveInput;

        private void Awake()
        {
            if (body == null)
            {
                body = GetComponent<Rigidbody2D>();
            }

            if (playerCarry == null)
            {
                playerCarry = GetComponent<PlayerCarry>();
            }
        }

        private void Update()
        {
            moveInput = ReadMoveInput();

            if (moveInput.sqrMagnitude > 0.001f)
            {
                FacingDirection = moveInput.normalized;
            }
        }

        private void FixedUpdate()
        {
            float speedMultiplier = 1f;

            if (playerCarry != null && playerCarry.HasCrop)
            {
                speedMultiplier = carrySpeedMultiplier;
            }

            Vector2 nextPosition = body.position + moveInput * (moveSpeed * speedMultiplier * Time.fixedDeltaTime);
            body.MovePosition(nextPosition);
        }

        private static Vector2 ReadMoveInput()
        {
            Vector2 input = Vector2.zero;
            Keyboard keyboard = Keyboard.current;

            if (keyboard != null)
            {
                if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
                {
                    input.x -= 1f;
                }

                if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
                {
                    input.x += 1f;
                }

                if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
                {
                    input.y -= 1f;
                }

                if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
                {
                    input.y += 1f;
                }
            }

            Gamepad gamepad = Gamepad.current;

            if (gamepad != null)
            {
                input += gamepad.leftStick.ReadValue();
            }

            return Vector2.ClampMagnitude(input, 1f);
        }
    }
}
