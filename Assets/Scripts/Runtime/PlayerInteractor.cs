using UnityEngine;
using UnityEngine.InputSystem;

namespace HarvestLeague
{
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private float interactionRadius = 1.2f;
        [SerializeField] private LayerMask interactableMask = ~0;
        [SerializeField] private Transform interactionOrigin;
        [SerializeField] private WorldPromptUI worldPromptUI;
        [SerializeField] private PlayerCarry playerCarry;
        [SerializeField] private PlayerWater playerWater;
        [SerializeField] private PlayerController playerController;

        private IInteractable currentInteractable;

        public PlayerCarry Carry
        {
            get { return playerCarry; }
        }

        public PlayerWater Water
        {
            get { return playerWater; }
        }

        public PlayerController Controller
        {
            get { return playerController; }
        }

        private void Awake()
        {
            if (interactionOrigin == null)
            {
                interactionOrigin = transform;
            }

            if (playerCarry == null)
            {
                playerCarry = GetComponent<PlayerCarry>();
            }

            if (playerWater == null)
            {
                playerWater = GetComponent<PlayerWater>();
            }

            if (playerController == null)
            {
                playerController = GetComponent<PlayerController>();
            }
        }

        private void Update()
        {
            currentInteractable = FindBestInteractable();
            UpdatePrompt();

            if (WasInteractPressed() && currentInteractable != null)
            {
                currentInteractable.Interact(this);
                currentInteractable = FindBestInteractable();
                UpdatePrompt();
            }
        }

        private IInteractable FindBestInteractable()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(interactionOrigin.position, interactionRadius, interactableMask);
            IInteractable bestInteractable = null;
            float bestScore = float.MinValue;

            for (int i = 0; i < hits.Length; i++)
            {
                if (!TryGetInteractable(hits[i], out IInteractable interactable))
                {
                    continue;
                }

                if (!interactable.CanInteract(this))
                {
                    continue;
                }

                Transform anchor = interactable.PromptAnchor != null ? interactable.PromptAnchor : hits[i].transform;
                Vector2 toTarget = (Vector2)(anchor.position - interactionOrigin.position);
                float distanceScore = -toTarget.sqrMagnitude;
                float facingScore = 0f;

                if (playerController != null && toTarget.sqrMagnitude > 0.0001f)
                {
                    facingScore = Vector2.Dot(playerController.FacingDirection.normalized, toTarget.normalized);
                }

                float combinedScore = distanceScore + (facingScore * 0.5f);

                if (combinedScore > bestScore)
                {
                    bestScore = combinedScore;
                    bestInteractable = interactable;
                }
            }

            return bestInteractable;
        }

        private void UpdatePrompt()
        {
            if (worldPromptUI == null)
            {
                return;
            }

            if (currentInteractable == null)
            {
                worldPromptUI.Clear();
                return;
            }

            string prompt = currentInteractable.GetPrompt(this);

            if (string.IsNullOrWhiteSpace(prompt))
            {
                worldPromptUI.Clear();
                return;
            }

            worldPromptUI.SetPrompt(currentInteractable.PromptAnchor, prompt);
        }

        private static bool WasInteractPressed()
        {
            Keyboard keyboard = Keyboard.current;

            if (keyboard != null && keyboard.eKey.wasPressedThisFrame)
            {
                return true;
            }

            Gamepad gamepad = Gamepad.current;
            return gamepad != null && gamepad.buttonSouth.wasPressedThisFrame;
        }

        private static bool TryGetInteractable(Collider2D hit, out IInteractable interactable)
        {
            MonoBehaviour[] components = hit.GetComponents<MonoBehaviour>();

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] is IInteractable found)
                {
                    interactable = found;
                    return true;
                }
            }

            MonoBehaviour[] parentComponents = hit.GetComponentsInParent<MonoBehaviour>();

            for (int i = 0; i < parentComponents.Length; i++)
            {
                if (parentComponents[i] is IInteractable found)
                {
                    interactable = found;
                    return true;
                }
            }

            interactable = null;
            return false;
        }

        private void OnDrawGizmosSelected()
        {
            Transform origin = interactionOrigin != null ? interactionOrigin : transform;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(origin.position, interactionRadius);
        }
    }
}
