using TMPro;
using UnityEngine;

namespace HarvestLeague
{
    public class ResultPanelController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private string victoryText = "VICTORY";
        [SerializeField] private string defeatText = "DEFEAT";

        private MatchManager boundMatchManager;

        private void Awake()
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }

            HideImmediate();
        }

        private void Update()
        {
            if (boundMatchManager == null && MatchManager.Instance != null)
            {
                boundMatchManager = MatchManager.Instance;
                boundMatchManager.MatchEnded += HandleMatchEnded;
            }
        }

        private void OnDestroy()
        {
            if (boundMatchManager != null)
            {
                boundMatchManager.MatchEnded -= HandleMatchEnded;
            }
        }

        private void HandleMatchEnded(bool victory)
        {
            Show(victory ? victoryText : defeatText);
        }

        private void Show(string message)
        {
            if (resultText != null)
            {
                resultText.text = message;
            }

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }

        private void HideImmediate()
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }
    }
}
