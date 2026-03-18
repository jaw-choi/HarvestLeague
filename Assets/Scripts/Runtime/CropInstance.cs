using UnityEngine;

namespace HarvestLeague
{
    [System.Serializable]
    public class CropInstance
    {
        public CropData Data { get; private set; }
        public float Growth { get; private set; }
        public CropStage Stage { get; private set; } = CropStage.Seed;

        public bool IsPlanted
        {
            get { return Data != null; }
        }

        public bool IsReady
        {
            get { return IsPlanted && Stage == CropStage.Ready; }
        }

        public float NormalizedGrowth
        {
            get
            {
                if (!IsPlanted || Data.maxGrowth <= 0f)
                {
                    return 0f;
                }

                return Mathf.Clamp01(Growth / Data.maxGrowth);
            }
        }

        public void Plant(CropData data)
        {
            Data = data;
            Growth = 0f;
            UpdateStage();
        }

        public bool Tick(float deltaTime)
        {
            if (!IsPlanted || IsReady)
            {
                return false;
            }

            return AddGrowth(Data.sunlightRate * deltaTime);
        }

        public bool AddGrowth(float amount)
        {
            if (!IsPlanted || amount <= 0f)
            {
                return false;
            }

            CropStage previousStage = Stage;
            Growth = Mathf.Clamp(Growth + amount, 0f, Data.maxGrowth);
            UpdateStage();
            return previousStage != Stage;
        }

        public void Clear()
        {
            Data = null;
            Growth = 0f;
            Stage = CropStage.Seed;
        }

        private void UpdateStage()
        {
            if (!IsPlanted)
            {
                Stage = CropStage.Seed;
                return;
            }

            float normalized = NormalizedGrowth;

            if (normalized >= 1f)
            {
                Stage = CropStage.Ready;
            }
            else if (normalized >= 0.85f)
            {
                Stage = CropStage.Mature;
            }
            else if (normalized >= 0.5f)
            {
                Stage = CropStage.Mid;
            }
            else if (normalized >= 0.25f)
            {
                Stage = CropStage.Sprout;
            }
            else
            {
                Stage = CropStage.Seed;
            }
        }
    }
}
