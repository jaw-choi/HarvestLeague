using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HarvestLeague
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text coinText;
        [SerializeField] private TMP_Text goalText;
        [SerializeField] private TMP_Text waterText;
        [SerializeField] private Image heldItemIcon;
        [SerializeField] private GameObject heldItemRoot;
        [SerializeField] private PlayerCarry playerCarry;
        [SerializeField] private PlayerWater playerWater;

        private ScoreManager boundScoreManager;
        private MatchManager boundMatchManager;
        private bool carryBound;
        private bool waterBound;

        private void Start()
        {
            TryBindReferences();
            RefreshAll();
        }

        private void Update()
        {
            TryBindReferences();
        }

        private void OnDestroy()
        {
            UnbindAll();
        }

        private void TryBindReferences()
        {
            if (boundScoreManager == null && ScoreManager.Instance != null)
            {
                boundScoreManager = ScoreManager.Instance;
                boundScoreManager.ScoreChanged += HandleScoreChanged;
                boundScoreManager.CoinChanged += HandleCoinChanged;
                boundScoreManager.GoalChanged += HandleGoalChanged;
                RefreshScore(boundScoreManager.Score);
                RefreshCoin(boundScoreManager.Coin);
                RefreshGoal(boundScoreManager.GoalScore);
            }

            if (boundMatchManager == null && MatchManager.Instance != null)
            {
                boundMatchManager = MatchManager.Instance;
                boundMatchManager.TimerChanged += HandleTimerChanged;
                RefreshTimer(boundMatchManager.RemainingTime);
            }

            if (!carryBound)
            {
                if (playerCarry == null)
                {
                    playerCarry = Object.FindFirstObjectByType<PlayerCarry>();
                }

                if (playerCarry != null)
                {
                    playerCarry.CarryChanged += HandleCarryChanged;
                    carryBound = true;
                    RefreshHeldItem(playerCarry.CurrentItem);
                }
            }

            if (!waterBound)
            {
                if (playerWater == null)
                {
                    playerWater = Object.FindFirstObjectByType<PlayerWater>();
                }

                if (playerWater != null)
                {
                    playerWater.WaterChanged += HandleWaterChanged;
                    waterBound = true;
                    RefreshWater(playerWater.CurrentCharge, playerWater.MaxCharge);
                }
            }
        }

        private void UnbindAll()
        {
            if (boundScoreManager != null)
            {
                boundScoreManager.ScoreChanged -= HandleScoreChanged;
                boundScoreManager.CoinChanged -= HandleCoinChanged;
                boundScoreManager.GoalChanged -= HandleGoalChanged;
            }

            if (boundMatchManager != null)
            {
                boundMatchManager.TimerChanged -= HandleTimerChanged;
            }

            if (carryBound && playerCarry != null)
            {
                playerCarry.CarryChanged -= HandleCarryChanged;
            }

            if (waterBound && playerWater != null)
            {
                playerWater.WaterChanged -= HandleWaterChanged;
            }
        }

        private void RefreshAll()
        {
            if (boundScoreManager != null)
            {
                RefreshScore(boundScoreManager.Score);
                RefreshCoin(boundScoreManager.Coin);
                RefreshGoal(boundScoreManager.GoalScore);
            }

            if (boundMatchManager != null)
            {
                RefreshTimer(boundMatchManager.RemainingTime);
            }

            if (playerCarry != null)
            {
                RefreshHeldItem(playerCarry.CurrentItem);
            }

            if (playerWater != null)
            {
                RefreshWater(playerWater.CurrentCharge, playerWater.MaxCharge);
            }
        }

        private void HandleScoreChanged(int value)
        {
            RefreshScore(value);
        }

        private void HandleCoinChanged(int value)
        {
            RefreshCoin(value);
        }

        private void HandleGoalChanged(int value)
        {
            RefreshGoal(value);
        }

        private void HandleTimerChanged(float value)
        {
            RefreshTimer(value);
        }

        private void HandleCarryChanged(CarryItem item)
        {
            RefreshHeldItem(item);
        }

        private void HandleWaterChanged(int current, int max)
        {
            RefreshWater(current, max);
        }

        private void RefreshTimer(float timeValue)
        {
            if (timerText == null)
            {
                return;
            }

            int totalSeconds = Mathf.Max(0, Mathf.CeilToInt(timeValue));
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        private void RefreshScore(int value)
        {
            if (scoreText != null)
            {
                scoreText.text = string.Format("Score {0}", value);
            }
        }

        private void RefreshCoin(int value)
        {
            if (coinText != null)
            {
                coinText.text = string.Format("Coin {0}", value);
            }
        }

        private void RefreshGoal(int value)
        {
            if (goalText != null)
            {
                goalText.text = string.Format("Goal {0}", value);
            }
        }

        private void RefreshWater(int current, int max)
        {
            if (waterText != null)
            {
                waterText.text = string.Format("Water {0}/{1}", current, max);
            }
        }

        private void RefreshHeldItem(CarryItem item)
        {
            if (heldItemRoot != null)
            {
                heldItemRoot.SetActive(item != null && !item.IsEmpty && item.cropData != null);
            }

            if (heldItemIcon == null)
            {
                return;
            }

            if (item == null || item.IsEmpty || item.cropData == null)
            {
                heldItemIcon.enabled = false;
                heldItemIcon.sprite = null;
                return;
            }

            heldItemIcon.enabled = true;
            heldItemIcon.sprite = item.type == CarryItemType.Seed ? item.cropData.seedSprite : item.cropData.readySprite;
        }
    }
}
