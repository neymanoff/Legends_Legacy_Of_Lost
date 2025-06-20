// Assets/Scripts/UI/FloatingText.cs
using TMPro;
using UnityEngine;

namespace Game.UI {
    public class FloatingText : MonoBehaviour {
        [SerializeField] private TMP_Text textMesh;
        [SerializeField] private float floatSpeed = 20f;
        [SerializeField] private float lifetime = 1.5f;

        private float timer;
        private Vector3 moveDirection = Vector3.up;
        private Color originalColor;

        private void Awake() {
            if (textMesh == null)
                textMesh = GetComponentInChildren<TMP_Text>();

            originalColor = textMesh.color;
        }

        public void Show(string content, Color color, bool isCrit) {
            textMesh.text = content;
            textMesh.color = color;

            if (isCrit) {
                textMesh.fontSize *= 1.3f;
                textMesh.fontStyle = FontStyles.Bold;
            } else {
                textMesh.fontSize = 36f; // your default size
                textMesh.fontStyle = FontStyles.Normal;
            }

            timer = 0f;
            originalColor = textMesh.color;
        }

        private void Update() {
            timer += Time.deltaTime;

            // Move upward
            transform.position += moveDirection * floatSpeed * Time.deltaTime;

            // Fade out
            float alpha = Mathf.Lerp(originalColor.a, 0, timer / lifetime);
            textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            // Destroy when done
            if (timer >= lifetime)
                Destroy(gameObject);
        }
    }
}
