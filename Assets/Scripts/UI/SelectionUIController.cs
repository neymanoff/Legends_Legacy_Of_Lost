using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Controls unit/grid selection UI
namespace UI
{
    public
        class SelectionUIController : MonoBehaviour
    {
        [Header("Units")]
        public GameObject unitButtonPrefab;            // prefab with UnitButtonHandler
        public Transform unitListContent;              // Content under ScrollView

        [Header("Grid Settings")]
        public TMP_Dropdown rowsDropdown;
        public TMP_Dropdown colsDropdown;

        [Header("Battle")]
        public Button startBattleButton;

        // Example list of available unit IDs (можно заменить на ScriptableObject list)
        public List<string> availableUnitIds = new List<string> { "Drogan", "Mara", "Selia", "Veles" };

        private void Awake()
        {
            PopulateUnitList();
            SetupDropdown(rowsDropdown, 3, 8, GameSession.Instance.GridRows);
            SetupDropdown(colsDropdown, 3, 8, GameSession.Instance.GridColumns);

            rowsDropdown.onValueChanged.AddListener(OnRowsChanged);
            colsDropdown.onValueChanged.AddListener(OnColsChanged);
            startBattleButton.onClick.AddListener(OnStartBattle);
        }

        void PopulateUnitList()
        {
            foreach (var id in availableUnitIds)
            {
                var go = Instantiate(unitButtonPrefab, unitListContent);
                var handler = go.GetComponent<UnitButtonHandler>();
                handler.Setup(id, OnUnitToggled);
            }
        }

        void SetupDropdown(TMP_Dropdown dd, int min, int max, int current)
        {
            dd.ClearOptions();
            var opts = new List<TMP_Dropdown.OptionData>();
            for (int i = min; i <= max; i++)
                opts.Add(new TMP_Dropdown.OptionData(i.ToString()));
            dd.AddOptions(opts);
            dd.value = Mathf.Clamp(current - min, 0, max - min);
        }

        public void OnUnitToggled(string unitId, bool isOn)
        {
            if (isOn)
                GameSession.Instance.SelectedUnitIds.Add(unitId);
            else
                GameSession.Instance.SelectedUnitIds.Remove(unitId);
        }

        public void OnRowsChanged(int index)
        {
            GameSession.Instance.GridRows = index + 3;
        }

        public void OnColsChanged(int index)
        {
            GameSession.Instance.GridColumns = index + 3;
        }

        public void OnStartBattle()
        {
            SceneManager.LoadScene("BattleScene");
        }
    }
}
