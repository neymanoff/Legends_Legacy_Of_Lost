    using Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Battle
{
    public class SkillButtonTooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private SkillInstance inst;
        private RectTransform rt;

        public void Init(SkillInstance instance)
        {
            inst = instance;
            rt = GetComponent<RectTransform>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Vector2 anchoredPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)SkillTooltip.Instance.transform.parent,
                rt.position,
                eventData.enterEventCamera,
                out anchoredPos
            );
            SkillTooltip.Instance.Show(
                inst.Definition,
                inst.LevelData,
                anchoredPos + new Vector2(0, rt.rect.height * 0.5f + 10) // чуть выше кнопки
            );
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SkillTooltip.Instance.Hide();
        }
    }
}