using UnityEngine;

namespace UI.Battle 
{
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

        [SerializeField] private FloatingTextPool textPool;
        [SerializeField] private Canvas worldCanvas;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                if (Application.isPlaying)
                    Destroy(gameObject);
                else
                    DestroyImmediate(gameObject);
                return;
            }
            
            _instance = this;
            
            if (Application.isPlaying)
                DontDestroyOnLoad(gameObject);

            if (textPool == null)
            {
                Debug.LogError("FloatingTextSpawner: текстовый пул не назначен!", this);
                enabled = false;
                return;
            }

            if (worldCanvas == null)
            {
                Debug.LogError("FloatingTextSpawner: Canvas не назначен!", this);
                enabled = false;
            }
        }

        public void Spawn(string content, Vector3 worldPosition, Color color, bool isCrit = false)
        {
            if (!enabled || textPool == null || worldCanvas == null)
                return;

            FloatingText floatingText = textPool.Get();
            floatingText.transform.SetParent(worldCanvas.transform);
            
            if (Camera.main != null)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
                floatingText.transform.position = screenPos;
            }

            floatingText.Show(content, color, isCrit);
        }
    }
}