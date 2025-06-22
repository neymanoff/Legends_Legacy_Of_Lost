using Core;
using TMPro;
using UnityEngine;

namespace UI.Battle
{
    public class SkillTooltip : MonoBehaviour
    {
        public static SkillTooltip Instance { get; private set; }

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text chanceText; 
        [SerializeField] private TMP_Text cooldownText;
        [SerializeField] private TMP_Text descText;
        private RectTransform rect;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            rect = (RectTransform)transform;
            Hide();
        }

        public void Show(Skill skill, SkillLevelData lvlData, Vector2 position)
        {
            nameText.text     = skill.SkillName;
            levelText.text    = $"Level: {lvlData.Level}";
            chanceText.text   = $"Chance: {lvlData.SuccessChance:P0}";
            cooldownText.text = $"Cooldown: {skill.Cooldown} turn{(skill.Cooldown>1?"s": "")}";
            descText.text     = skill.Description;

            rect.anchoredPosition    = position;
            canvasGroup.alpha        = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            canvasGroup.alpha        = 0f;
            canvasGroup.blocksRaycasts = false;
        }
    }
}