using System;
using UnityEngine;

namespace HarvestLeague
{
    public class MatchManager : MonoBehaviour
    {
        public static MatchManager Instance { get; private set; }

        [SerializeField] private float matchDuration = 45f;
        [SerializeField] private int startingCoin = 3;
        [SerializeField] private int goalScore = 300;
        [SerializeField] private bool autoStart = true;

        public event Action<float> TimerChanged;
        public event Action<bool> MatchEnded;

        public float RemainingTime { get; private set; }
        public bool IsPlaying { get; private set; }
        public bool HasEnded { get; private set; }

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
            if (autoStart)
            {
                StartMatch();
            }
        }

        private void Update()
        {
            if (!IsPlaying || HasEnded)
            {
                return;
            }

            RemainingTime = Mathf.Max(0f, RemainingTime - Time.deltaTime);
            TimerChanged?.Invoke(RemainingTime);

            if (RemainingTime <= 0f)
            {
                EndMatch();
            }
        }

        public void StartMatch()
        {
            RemainingTime = matchDuration;
            IsPlaying = true;
            HasEnded = false;

            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.ResetValues(startingCoin, goalScore);
            }

            TimerChanged?.Invoke(RemainingTime);
        }

        public void EndMatch()
        {
            if (HasEnded)
            {
                return;
            }

            RemainingTime = 0f;
            IsPlaying = false;
            HasEnded = true;

            bool victory = ScoreManager.Instance != null && ScoreManager.Instance.Score >= goalScore;
            TimerChanged?.Invoke(RemainingTime);
            MatchEnded?.Invoke(victory);
        }
    }
}
