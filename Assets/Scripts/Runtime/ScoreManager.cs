using System;
using UnityEngine;

namespace HarvestLeague
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        [SerializeField] private bool initializeOnStart;
        [SerializeField] private int fallbackStartingCoin = 3;
        [SerializeField] private int fallbackGoalScore = 300;

        public event Action<int> ScoreChanged;
        public event Action<int> CoinChanged;
        public event Action<int> GoalChanged;

        public int Score { get; private set; }
        public int Coin { get; private set; }
        public int GoalScore { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            if (initializeOnStart)
            {
                ResetValues(fallbackStartingCoin, fallbackGoalScore);
            }
        }

        public void ResetValues(int startingCoin, int goalScore)
        {
            Score = 0;
            Coin = Mathf.Max(0, startingCoin);
            GoalScore = Mathf.Max(0, goalScore);

            ScoreChanged?.Invoke(Score);
            CoinChanged?.Invoke(Coin);
            GoalChanged?.Invoke(GoalScore);
        }

        public bool TrySpendCoin(int amount)
        {
            if (amount <= 0)
            {
                return true;
            }

            if (Coin < amount)
            {
                return false;
            }

            Coin -= amount;
            CoinChanged?.Invoke(Coin);
            return true;
        }

        public void RegisterSale(CropData cropData)
        {
            if (cropData == null)
            {
                return;
            }

            Score += cropData.sellScore;
            Coin += cropData.sellCoin;

            ScoreChanged?.Invoke(Score);
            CoinChanged?.Invoke(Coin);
        }
    }
}
