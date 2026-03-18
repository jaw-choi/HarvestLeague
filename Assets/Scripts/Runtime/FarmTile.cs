using UnityEngine;
using UnityEngine.UI;

namespace HarvestLeague
{
    public class FarmTile : MonoBehaviour, IInteractable
    {
        [Header("References")]
        [SerializeField] private Transform promptAnchor;
        [SerializeField] private SpriteRenderer soilRenderer;
        [SerializeField] private SpriteRenderer cropRenderer;
        [SerializeField] private Image growthFillImage;
        [SerializeField] private GameObject growthBarRoot;
        [SerializeField] private GameObject readyIndicator;

        [Header("Soil Colors")]
        [SerializeField] private Color untilledColor = new Color(0.76f, 0.60f, 0.39f);
        [SerializeField] private Color tilledColor = new Color(0.45f, 0.27f, 0.12f);
        [SerializeField] private Color wetColor = new Color(0.28f, 0.20f, 0.12f);
        [SerializeField] private float wetFeedbackDuration = 0.75f;

        private readonly CropInstance crop = new CropInstance();
        private float wetTimer;

        public TileState State { get; private set; } = TileState.Untilled;

        public Transform PromptAnchor
        {
            get { return promptAnchor != null ? promptAnchor : transform; }
        }

        private void Awake()
        {
            if (soilRenderer == null)
            {
                soilRenderer = GetComponent<SpriteRenderer>();
            }
        }

        private void Start()
        {
            RefreshVisuals();
        }

        private void Update()
        {
            bool needsRefresh = false;

            if (State == TileState.Growing && crop.IsPlanted)
            {
                crop.Tick(Time.deltaTime);

                if (crop.IsReady)
                {
                    State = TileState.Ready;
                    needsRefresh = true;
                }
            }

            if (wetTimer > 0f)
            {
                wetTimer = Mathf.Max(0f, wetTimer - Time.deltaTime);
                needsRefresh = true;
            }

            UpdateGrowthVisuals();

            if (needsRefresh)
            {
                RefreshVisuals();
            }
        }

        public bool CanInteract(PlayerInteractor interactor)
        {
            if (interactor == null || interactor.Carry == null || interactor.Water == null)
            {
                return false;
            }

            switch (State)
            {
                case TileState.Untilled:
                    return interactor.Carry.IsEmpty;
                case TileState.Tilled:
                    return interactor.Carry.HasSeed;
                case TileState.Growing:
                    return crop.IsPlanted && interactor.Water.CurrentCharge > 0;
                case TileState.Ready:
                    return interactor.Carry.IsEmpty;
                default:
                    return false;
            }
        }

        public string GetPrompt(PlayerInteractor interactor)
        {
            switch (State)
            {
                case TileState.Untilled:
                    return "밭 갈기";
                case TileState.Tilled:
                    return interactor != null && interactor.Carry.HasSeed ? "씨뿌리기" : "씨앗 필요";
                case TileState.Growing:
                    return interactor != null && interactor.Water.CurrentCharge > 0 ? "물 주기" : "물 필요";
                case TileState.Ready:
                    return "수확";
                default:
                    return string.Empty;
            }
        }

        public void Interact(PlayerInteractor interactor)
        {
            if (interactor == null)
            {
                return;
            }

            switch (State)
            {
                case TileState.Untilled:
                    if (interactor.Carry.IsEmpty)
                    {
                        State = TileState.Tilled;
                    }
                    break;

                case TileState.Tilled:
                    if (interactor.Carry.HasSeed)
                    {
                        CarryItem seed = interactor.Carry.TakeItem();

                        if (seed != null && seed.cropData != null)
                        {
                            crop.Plant(seed.cropData);
                            State = TileState.Growing;
                        }
                    }
                    break;

                case TileState.Growing:
                    if (crop.IsPlanted && interactor.Water.TrySpendOne())
                    {
                        crop.AddGrowth(crop.Data.waterBonus);
                        wetTimer = wetFeedbackDuration;

                        if (crop.IsReady)
                        {
                            State = TileState.Ready;
                        }
                    }
                    break;

                case TileState.Ready:
                    if (crop.Data != null && interactor.Carry.TrySetCrop(crop.Data))
                    {
                        crop.Clear();
                        State = TileState.Untilled;
                        wetTimer = 0f;
                    }
                    break;
            }

            RefreshVisuals();
        }

        private void RefreshVisuals()
        {
            UpdateSoilVisual();
            UpdateGrowthVisuals();

            if (readyIndicator != null)
            {
                readyIndicator.SetActive(State == TileState.Ready);
            }
        }

        private void UpdateSoilVisual()
        {
            if (soilRenderer == null)
            {
                return;
            }

            Color baseColor = State == TileState.Untilled ? untilledColor : tilledColor;

            if (wetTimer > 0f && State != TileState.Untilled)
            {
                float t = wetFeedbackDuration <= 0f ? 1f : wetTimer / wetFeedbackDuration;
                baseColor = Color.Lerp(baseColor, wetColor, Mathf.Clamp01(t));
            }

            soilRenderer.color = baseColor;
        }

        private void UpdateGrowthVisuals()
        {
            if (cropRenderer != null)
            {
                bool showCrop = (State == TileState.Growing || State == TileState.Ready) && crop.Data != null;
                cropRenderer.enabled = showCrop;
                cropRenderer.sprite = showCrop ? crop.Data.GetSprite(crop.Stage) : null;
            }

            if (growthBarRoot != null)
            {
                growthBarRoot.SetActive((State == TileState.Growing || State == TileState.Ready) && crop.Data != null);
            }

            if (growthFillImage != null)
            {
                growthFillImage.fillAmount = crop.NormalizedGrowth;
            }
        }
    }
}
