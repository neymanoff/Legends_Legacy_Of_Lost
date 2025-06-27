using System.Collections.Generic;
using Core;
using TMPro;
using UI.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class SkillButtonsUI : MonoBehaviour
    {
        public static SkillButtonsUI Instance { get; private set; }
        [Header("UI References")]
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Transform buttonContainer;

        private UnitBase currentUnit;
        private UnitBase currentTarget;  
        private readonly List<SkillButton> skillButtons = new List<SkillButton>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void ShowButtons(UnitBase unit, UnitBase target)
        {
            Debug.Log("ShowButtons вызван"); // Добавьте эту строку
            currentTarget = target;
            Initialize(unit);
        }

        private void Initialize(UnitBase unit)
        {
            currentUnit = unit;
            ClearButtons();

            var instances = unit.SkillBook;
            for (int i = 0; i < instances.Count; i++)
            {
                var inst = instances[i];
                var go = Instantiate(buttonPrefab, buttonContainer);
                var btn = go.GetComponent<Button>();
                var tooltipHandler = go.AddComponent<SkillButtonTooltipHandler>();
                tooltipHandler.Init(inst);
                var iconImg = go.transform.Find("Icon").GetComponent<Image>();
                var overlayTf = go.transform.Find("CooldownOverlay");
                if (overlayTf == null)
                {
                    Debug.LogError($"[{nameof(SkillButtonsUI)}] В префабе нет дочернего объекта 'CooldownOverlay'");
                    return; // Здесь код прерывается, если не найден CooldownOverlay
                }
                var overlay = go.transform.Find("CooldownOverlay").GetComponent<Image>();
                var cdText = go.transform.Find("CooldownText").GetComponent<TMP_Text>();

                iconImg.sprite = inst.Definition.Icon;
                int index = i;
                btn.onClick.AddListener(() => OnSkillButtonClicked(index));
                btn.onClick.AddListener(() => {
                    Debug.Log($"[SkillButtonsUI] Clicked skill #{index}");
                    currentUnit.UseSkill(index, currentTarget);
                });

                skillButtons.Add(new SkillButton
                {
                    Button = btn,
                    Instance = inst,
                    Overlay = overlay,
                    CooldownText = cdText
                });
            }
        }

        private void OnSkillButtonClicked(int index)
        {
            Debug.Log($"[SkillButtonsUI] Clicked skill #{index}");
            currentUnit.UseSkill(index, currentTarget);
            currentUnit.TickSkillCooldowns();
            ClearButtons();
            TurnManager.Instance.FinishTurn();
        }

        private void Update()
        {
            foreach (var sb in skillButtons)
            {
                sb.Button.interactable = sb.Instance.IsReady;
                sb.Overlay.gameObject.SetActive(!sb.Instance.IsReady);
                sb.CooldownText.text = sb.Instance.RemainingCooldown.ToString();
            }
        }

        private void ClearButtons()
        {
            foreach (var sb in skillButtons)
                Destroy(sb.Button.gameObject);
            skillButtons.Clear();
        }

        private UnitBase ChooseTarget()
        {
            // TODO: реализовать логику выбора цели через GameManager или TargetSelector
            return null;
        }

        private class SkillButton
        {
            public Button Button;
            public SkillInstance Instance;
            public Image Overlay;
            public TMP_Text CooldownText;
        }
    }
}