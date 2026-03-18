using UnityEngine;

namespace HarvestLeague
{
    [CreateAssetMenu(menuName = "HarvestLeague/Crop Data", fileName = "CropData")]
    public class CropData : ScriptableObject
    {
        public string cropId;
        public string displayName;
        public int seedCost = 1;
        public int sellScore = 100;
        public int sellCoin = 1;
        public float maxGrowth = 100f;
        public float sunlightRate = 10f;
        public float waterBonus = 35f;
        public Sprite seedSprite;
        public Sprite sproutSprite;
        public Sprite midSprite;
        public Sprite matureSprite;
        public Sprite readySprite;

        public Sprite GetSprite(CropStage stage)
        {
            switch (stage)
            {
                case CropStage.Seed:
                    return seedSprite;
                case CropStage.Sprout:
                    return sproutSprite;
                case CropStage.Mid:
                    return midSprite;
                case CropStage.Mature:
                    return matureSprite;
                case CropStage.Ready:
                    return readySprite;
                default:
                    return seedSprite;
            }
        }
    }
}
