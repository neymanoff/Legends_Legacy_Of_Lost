// Assets/Scripts/UI/FloatingTextSpawner.cs

using Game.UI;
using UnityEngine;

namespace UI.Battle {
    public class FloatingTextSpawner : MonoBehaviour
    {
        private static FloatingTextSpawner _instance;
        public static FloatingTextSpawner Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<FloatingTextSpawner>();
                if (_instance != null) return _instance;
                var go = new GameObject(nameof(FloatingTextSpawner));
                _instance = go.AddComponent<FloatingTextSpawner>();
                DontDestroyOnLoad(go);
                return _instance;
            }
        }

        [SerializeField] private GameObject floatingTextPrefab;
        [SerializeField] private Canvas worldCanvas;

        private void Awake() {
            if (Instance != null && Instance != this) {
                if (Application.isPlaying)
                    Destroy(gameObject);
                else
                    DestroyImmediate(gameObject);

                return;
            }
            _instance = this;
            if (Application.isPlaying)
                DontDestroyOnLoad(gameObject);
        }

        public void Spawn(string content, Vector3 worldPosition, Color color, bool isCrit = false) {
            if (floatingTextPrefab == null || worldCanvas == null) {
                Debug.LogWarning("FloatingTextSpawner: Missing prefab or canvas.");
                return;
            }

            var go = Instantiate(floatingTextPrefab, worldCanvas.transform);
            if (Camera.main != null)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
                go.transform.position = screenPos;
            }

            var floatingText = go.GetComponent<FloatingText>();
            if (floatingText != null) {
                floatingText.Show(content, color, isCrit);
            } else {
                Debug.LogWarning("FloatingTextSpawner: No FloatingText component found.");
            }
        }
    }
}
