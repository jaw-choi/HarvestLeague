using UnityEngine;

namespace HarvestLeague
{
    public class SellStationInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform promptAnchor;
        [SerializeField] private Transform popupSpawnPoint;
        [SerializeField] private FloatingText floatingTextPrefab;
        [SerializeField] private Color popupColor = new Color(1f, 0.86f, 0.25f);

        public Transform PromptAnchor
        {
            get { return promptAnchor != null ? promptAnchor : transform; }
        }

        public bool CanInteract(PlayerInteractor interactor)
        {
            return interactor != null && interactor.Carry != null && interactor.Carry.HasCrop;
        }

        public string GetPrompt(PlayerInteractor interactor)
        {
            return "판매";
        }

        public void Interact(PlayerInteractor interactor)
        {
            if (interactor == null || interactor.Carry == null || !interactor.Carry.HasCrop)
            {
                return;
            }

            CarryItem item = interactor.Carry.TakeItem();

            if (item == null || item.cropData == null)
            {
                return;
            }

            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.RegisterSale(item.cropData);
            }

            SpawnPopup(string.Format("+{0}", item.cropData.sellScore));
        }

        private void SpawnPopup(string message)
        {
            if (floatingTextPrefab == null)
            {
                return;
            }

            Transform anchor = popupSpawnPoint != null ? popupSpawnPoint : transform;
            FloatingText instance = Instantiate(floatingTextPrefab, anchor.position, Quaternion.identity);
            instance.Initialize(message, popupColor);
        }
    }
}
