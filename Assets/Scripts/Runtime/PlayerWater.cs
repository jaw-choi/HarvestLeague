using System;
using UnityEngine;

namespace HarvestLeague
{
    public class PlayerWater : MonoBehaviour
    {
        [SerializeField] private int maxCharge = 2;
        [SerializeField] private int startingCharge;

        public event Action<int, int> WaterChanged;

        public int MaxCharge
        {
            get { return maxCharge; }
        }

        public int CurrentCharge { get; private set; }

        private void Awake()
        {
            CurrentCharge = Mathf.Clamp(startingCharge, 0, maxCharge);
            NotifyChanged();
        }

        public void RefillFull()
        {
            CurrentCharge = maxCharge;
            NotifyChanged();
        }

        public bool TrySpendOne()
        {
            if (CurrentCharge <= 0)
            {
                return false;
            }

            CurrentCharge -= 1;
            NotifyChanged();
            return true;
        }

        private void NotifyChanged()
        {
            WaterChanged?.Invoke(CurrentCharge, maxCharge);
        }
    }
}
