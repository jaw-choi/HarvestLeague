using UnityEngine;

namespace HarvestLeague
{
    public interface IInteractable
    {
        Transform PromptAnchor { get; }
        bool CanInteract(PlayerInteractor interactor);
        string GetPrompt(PlayerInteractor interactor);
        void Interact(PlayerInteractor interactor);
    }
}
