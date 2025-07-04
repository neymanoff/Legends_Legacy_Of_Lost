// Assets/Scripts/UI/Battle/FloatingText.cs
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using FontStyles = TMPro.FontStyles;

namespace UI.Battle 
{
    public class FloatingText : MonoBehaviour 
    {
        [SerializeField] private TMP_Text textMesh;
        [SerializeField] private float floatSpeed = 20f;
        [SerializeField] private float lifetime = 1.5f;

        private float timer;
        private static readonly Vector3 moveDirection = Vector3.up;
        private Color originalColor;
        private FloatingTextPool pool;

        private void Awake() 
        {
            if (textMesh == null)
                textMesh = GetComponentInChildren<TMP_Text>();

            originalColor = textMesh.color;
        }

        public void Initialize(FloatingTextPool pool)
        {
            if (pool == null)
                Debug.LogError("FloatingTextPool не может быть null!", this);
                
            this.pool = pool;
        }

        public void Show(string content, Color color, bool isCrit) 
        {
            textMesh.text = content;
            textMesh.color = color;

            if (isCrit) 
            {
                textMesh.fontSize *= 1.3f;
                textMesh.fontStyle = FontStyles.Bold;
            } 
            else 
            {
                textMesh.fontSize = 36f;
                textMesh.fontStyle = FontStyles.Normal;
            }

            timer = 0f;
            originalColor = textMesh.color;
            gameObject.SetActive(true);
        }

        private void Update() 
        {
            if (!pool)
            {
                Debug.LogError("FloatingTextPool не инициализирован! Убедитесь, что вызван метод Initialize.", this);
                enabled = false;
                return;
            }

            timer += Time.deltaTime;

            transform.position += moveDirection * (floatSpeed * Time.deltaTime);

            float alpha = Mathf.Lerp(originalColor.a, 0, timer / lifetime);
            textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            if (timer >= lifetime)
                pool.Return(this);
        }
    }
}