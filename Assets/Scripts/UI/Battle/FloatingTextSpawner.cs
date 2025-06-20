// Assets/Scripts/UI/FloatingTextSpawner.cs
using UnityEngine;

namespace Game.UI {
    public class FloatingTextSpawner : MonoBehaviour {
        public static FloatingTextSpawner Instance { get; private set; }

        [SerializeField] private GameObject floatingTextPrefab;
        [SerializeField] private Canvas worldCanvas;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void Spawn(string content, Vector3 worldPosition, Color color, bool isCrit = false) {
            if (floatingTextPrefab == null || worldCanvas == null) {
                Debug.LogWarning("FloatingTextSpawner: Missing prefab or canvas.");
                return;
            }

            GameObject go = Instantiate(floatingTextPrefab, worldCanvas.transform);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
            go.transform.position = screenPos;

            FloatingText floatingText = go.GetComponent<FloatingText>();
            if (floatingText != null) {
                floatingText.Show(content, color, isCrit);
            } else {
                Debug.LogWarning("FloatingTextSpawner: No FloatingText component found.");
            }
        }
    }
}
