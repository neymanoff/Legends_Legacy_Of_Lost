using UnityEngine;

namespace Core
{
    public struct SkillFeedback
    {
        public UnitBase Target;
        public string Text;
        public Color Color;
        
        public SkillFeedback(UnitBase target, string text, Color color)
        {
            Target = target;
            Text = text;
            Color = color;
        }
    }
}