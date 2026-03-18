using System;

namespace HarvestLeague
{
    public enum TileState
    {
        Untilled,
        Tilled,
        Growing,
        Ready
    }

    public enum CropStage
    {
        Seed,
        Sprout,
        Mid,
        Mature,
        Ready
    }

    public enum CarryItemType
    {
        None,
        Seed,
        Crop
    }

    [Serializable]
    public class CarryItem
    {
        public CarryItemType type = CarryItemType.None;
        public CropData cropData;

        public bool IsEmpty
        {
            get { return type == CarryItemType.None || cropData == null; }
        }

        public static CarryItem None()
        {
            return new CarryItem();
        }

        public static CarryItem CreateSeed(CropData data)
        {
            return new CarryItem
            {
                type = CarryItemType.Seed,
                cropData = data
            };
        }

        public static CarryItem CreateCrop(CropData data)
        {
            return new CarryItem
            {
                type = CarryItemType.Crop,
                cropData = data
            };
        }
    }
}
