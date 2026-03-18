using TMPro;
using UnityEngine;

namespace HarvestLeague
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] private TMP_Text textLabel;
        [SerializeField] private float lifeTime = 1f;
        [SerializeField] private float riseDistance = 0.6f;

        private float age;
        private Vector3 startPosition;
        private Color startColor = Color.white;

        private void Awake()
        {
            if (textLabel == null)
            {
                textLabel = GetComponentInChildren<TMP_Text>();
            }
        }

        private void OnEnable()
        {
            age = 0f;
            startPosition = transform.position;

            if (textLabel != null)
            {
                startColor = textLabel.color;
            }
        }

        private void Update()
        {
            age += Time.deltaTime;
            float t = lifeTime <= 0f ? 1f : Mathf.Clamp01(age / lifeTime);
            transform.position = startPosition + (Vector3.up * (riseDistance * t));

            if (textLabel != null)
            {
                Color currentColor = startColor;
                currentColor.a = 1f - t;
                textLabel.color = currentColor;
            }

            if (age >= lifeTime)
            {
                Destroy(gameObject);
            }
        }

        public void Initialize(string message, Color color)
        {
            if (textLabel == null)
            {
                textLabel = GetComponentInChildren<TMP_Text>();
            }

            if (textLabel != null)
            {
                textLabel.text = message;
                textLabel.color = color;
                startColor = color;
            }
        }
    }
}
