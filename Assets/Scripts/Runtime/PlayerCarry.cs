using System;
using UnityEngine;

namespace HarvestLeague
{
    public class PlayerCarry : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer heldItemRenderer;

        private CarryItem currentItem = CarryItem.None();

        public event Action<CarryItem> CarryChanged;

        public CarryItem CurrentItem
        {
            get { return currentItem; }
        }

        public bool IsEmpty
        {
            get { return currentItem == null || currentItem.IsEmpty; }
        }

        public bool HasSeed
        {
            get { return !IsEmpty && currentItem.type == CarryItemType.Seed; }
        }

        public bool HasCrop
        {
            get { return !IsEmpty && currentItem.type == CarryItemType.Crop; }
        }

        public CropData CurrentCropData
        {
            get { return IsEmpty ? null : currentItem.cropData; }
        }

        private void Awake()
        {
            UpdateVisual();
        }

        public bool TrySetSeed(CropData cropData)
        {
            if (!IsEmpty || cropData == null)
            {
                return false;
            }

            currentItem = CarryItem.CreateSeed(cropData);
            NotifyChanged();
            return true;
        }

        public bool TrySetCrop(CropData cropData)
        {
            if (!IsEmpty || cropData == null)
            {
                return false;
            }

            currentItem = CarryItem.CreateCrop(cropData);
            NotifyChanged();
            return true;
        }

        public CarryItem TakeItem()
        {
            CarryItem item = currentItem;
            currentItem = CarryItem.None();
            NotifyChanged();
            return item;
        }

        public void Clear()
        {
            currentItem = CarryItem.None();
            NotifyChanged();
        }

        private void NotifyChanged()
        {
            UpdateVisual();
            CarryChanged?.Invoke(currentItem);
        }

        private void UpdateVisual()
        {
            if (heldItemRenderer == null)
            {
                return;
            }

            if (IsEmpty || currentItem.cropData == null)
            {
                heldItemRenderer.enabled = false;
                heldItemRenderer.sprite = null;
                return;
            }

            heldItemRenderer.enabled = true;
            heldItemRenderer.sprite = currentItem.type == CarryItemType.Seed
                ? currentItem.cropData.seedSprite
                : currentItem.cropData.readySprite;
        }
    }
}
