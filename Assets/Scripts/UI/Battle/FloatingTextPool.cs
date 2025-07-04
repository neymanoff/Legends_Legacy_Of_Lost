using System.Collections.Generic;
using UnityEngine;

namespace UI.Battle
{
    public class FloatingTextPool : MonoBehaviour
    {
        [SerializeField] private FloatingText prefab;
        [SerializeField] private int initialPoolSize = 10;
        private Queue<FloatingText> availableTexts;

        private void Awake()
        {
            availableTexts = new Queue<FloatingText>();
            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateNewText();
            }
        }

        private void CreateNewText()
        {
            var text = Instantiate(prefab, transform);
            text.Initialize(this);
            text.gameObject.SetActive(false);
            availableTexts.Enqueue(text);
        }

        public FloatingText Get()
        {
            if (availableTexts.Count == 0) CreateNewText();
            return availableTexts.Dequeue();
        }
        
        public void Return(FloatingText text)
        {
            text.gameObject.SetActive(false);
            availableTexts.Enqueue(text);
        }
    }
}