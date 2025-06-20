using Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Skills;

namespace Game.UI {
    /// <summary>
    /// UI-контроллер для показа кнопок навыков игрока.
    /// </summary>
    public class SkillButtonsUI : MonoBehaviour {
        public static SkillButtonsUI Instance { get; private set; }

        [SerializeField] private Transform buttonPanel;     // Панель, куда инстанцируются кнопки
        [SerializeField] private GameObject buttonPrefab;   // Префаб кнопки (должен иметь Button + TMP_Text)

        private PlayerUnit currentPlayer;
        private UnitBase currentTarget;

        private void Awake() {
            // Синглтон
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            // Скрываем саму панель по умолчанию
            buttonPanel.gameObject.SetActive(false);
        }

        /// <summary>
        /// Показать кнопки навыков для данного игрока и цели.
        /// </summary>
        public void ShowButtons(PlayerUnit player, UnitBase target) {
            currentPlayer = player;
            currentTarget = target;

            // Очистить старые
            ClearButtons();

            // Получаем массив навыков из PlayerUnit
            Skill[] skills = player.GetSkills();
            for (int i = 0; i < skills.Length; i++) {
                int index = i;
                GameObject btnObj = Instantiate(buttonPrefab, buttonPanel);

                // Подпись или иконка
                var label = btnObj.GetComponentInChildren<TMP_Text>();
                if (label != null) label.text = skills[index].skillName;

                // Обработчик клика
                var btn = btnObj.GetComponent<Button>();
                if (btn != null) {
                    btn.onClick.AddListener(() => {
                        // Выбираем скилл, запускаем ход и прячем кнопки
                        currentPlayer.SetSelectedSkill(index);
                        currentPlayer.TakeTurn(currentTarget);
                        HideButtons();
                    });
                }
            }

            // Делаем панель видимой и интерактивной
            buttonPanel.gameObject.SetActive(true);
        }

        /// <summary>
        /// Скрыть все кнопки и саму панель.
        /// </summary>
        public void HideButtons() {
            buttonPanel.gameObject.SetActive(false);
            ClearButtons();
        }

        /// <summary>
        /// Удаляет все дочерние объекты из buttonPanel.
        /// </summary>
        private void ClearButtons() {
            for (int i = buttonPanel.childCount - 1; i >= 0; i--) {
                Destroy(buttonPanel.GetChild(i).gameObject);
            }
        }
    }
}
