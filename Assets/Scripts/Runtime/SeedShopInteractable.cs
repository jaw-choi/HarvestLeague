using UnityEngine;

namespace HarvestLeague
{
    public class SeedShopInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private CropData[] stock;
        [SerializeField] private int selectedIndex;
        [SerializeField] private bool autoAdvanceSelection = true;
        [SerializeField] private Transform promptAnchor;
        [SerializeField] private SpriteRenderer previewRenderer;

        public Transform PromptAnchor
        {
            get { return promptAnchor != null ? promptAnchor : transform; }
        }

        private CropData CurrentCrop
        {
            get
            {
                if (stock == null || stock.Length == 0)
                {
                    return null;
                }

                int clampedIndex = Mathf.Clamp(selectedIndex, 0, stock.Length - 1);
                return stock[clampedIndex];
            }
        }

        private void Start()
        {
            UpdatePreview();
        }

        public bool CanInteract(PlayerInteractor interactor)
        {
            CropData crop = CurrentCrop;

            return interactor != null
                && crop != null
                && interactor.Carry != null
                && interactor.Carry.IsEmpty
                && ScoreManager.Instance != null
                && ScoreManager.Instance.Coin >= crop.seedCost;
        }

        public string GetPrompt(PlayerInteractor interactor)
        {
            CropData crop = CurrentCrop;

            if (crop == null)
            {
                return string.Empty;
            }

            if (ScoreManager.Instance != null && ScoreManager.Instance.Coin < crop.seedCost)
            {
                return "코인 부족";
            }

            return string.Format("{0} 씨앗 구매 ({1})", crop.displayName, crop.seedCost);
        }

        public void Interact(PlayerInteractor interactor)
        {
            CropData crop = CurrentCrop;

            if (crop == null || interactor == null || interactor.Carry == null || !interactor.Carry.IsEmpty)
            {
                return;
            }

            if (ScoreManager.Instance == null || !ScoreManager.Instance.TrySpendCoin(crop.seedCost))
            {
                return;
            }

            interactor.Carry.TrySetSeed(crop);

            if (autoAdvanceSelection && stock != null && stock.Length > 1)
            {
                selectedIndex = (selectedIndex + 1) % stock.Length;
                UpdatePreview();
            }
        }

        private void UpdatePreview()
        {
            if (previewRenderer == null)
            {
                return;
            }

            CropData crop = CurrentCrop;
            Sprite previewSprite = null;

            if (crop != null)
            {
                previewSprite = crop.seedSprite != null ? crop.seedSprite : crop.readySprite;
            }

            previewRenderer.enabled = previewSprite != null;
            previewRenderer.sprite = previewSprite;
        }
    }
}
