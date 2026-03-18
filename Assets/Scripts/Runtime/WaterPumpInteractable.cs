using UnityEngine;

namespace HarvestLeague
{
    public class WaterPumpInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform promptAnchor;

        public Transform PromptAnchor
        {
            get { return promptAnchor != null ? promptAnchor : transform; }
        }

        public bool CanInteract(PlayerInteractor interactor)
        {
            return interactor != null && interactor.Water != null;
        }

        public string GetPrompt(PlayerInteractor interactor)
        {
            return "물 채우기";
        }

        public void Interact(PlayerInteractor interactor)
        {
            if (interactor == null || interactor.Water == null)
            {
                return;
            }

            interactor.Water.RefillFull();
        }
    }
}
