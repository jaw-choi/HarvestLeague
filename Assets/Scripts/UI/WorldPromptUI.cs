using TMPro;
using UnityEngine;

namespace HarvestLeague
{
    public class WorldPromptUI : MonoBehaviour
    {
        [SerializeField] private RectTransform root;
        [SerializeField] private TMP_Text promptText;
        [SerializeField] private Vector3 screenOffset = new Vector3(0f, 48f, 0f);
        [SerializeField] private Camera targetCamera;

        private Transform target;

        private void Awake()
        {
            if (root == null)
            {
                root = transform as RectTransform;
            }

            if (targetCamera == null)
            {
                targetCamera = Camera.main;
            }

            Clear();
        }

        private void LateUpdate()
        {
            if (root == null || target == null || targetCamera == null)
            {
                return;
            }

            Vector3 screenPoint = targetCamera.WorldToScreenPoint(target.position);

            if (screenPoint.z <= 0f)
            {
                root.gameObject.SetActive(false);
                return;
            }

            root.gameObject.SetActive(true);
            root.position = screenPoint + screenOffset;
        }

        public void SetPrompt(Transform targetTransform, string message)
        {
            target = targetTransform;

            if (promptText != null)
            {
                promptText.text = message;
            }

            if (root != null)
            {
                root.gameObject.SetActive(target != null && !string.IsNullOrWhiteSpace(message));
            }
        }

        public void Clear()
        {
            target = null;

            if (promptText != null)
            {
                promptText.text = string.Empty;
            }

            if (root != null)
            {
                root.gameObject.SetActive(false);
            }
        }
    }
}
