using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Handles one button in the unit list
namespace UI
{
    public class UnitButtonHandler : MonoBehaviour
    {
        public Toggle toggle;        // or Button if you prefer switch logic
        public TMP_Text label;       // для отображения названия/иконки

        private string unitId;
        private System.Action<string, bool> callback;

        public void Setup(string id, System.Action<string, bool> onToggle)
        {
            unitId = id;
            label.text = id;  // позже можно заменить на иконку
            callback = onToggle;
            toggle.onValueChanged.AddListener(OnValueChanged);
        }

        void OnValueChanged(bool isOn)
        {
            callback?.Invoke(unitId, isOn);
        }
    }
}