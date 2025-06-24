using System.Collections.Generic;
using Core;
using Game.UI;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Basic Attack", menuName = "Skills/Basic Attack")]
    public class BasicAttackSkill : Skill
    {
        public override List<SkillFeedback> Apply(UnitBase caster, UnitBase target, float powerMultiplier)
        {
            int damage = Mathf.RoundToInt(BaseAmount * powerMultiplier);
            var feedbacks = new List<SkillFeedback>();
            if (damage > 0) 
                feedbacks.Add(new SkillFeedback(target, $"{damage}", Color.white));
            else
                feedbacks.Add(new SkillFeedback(caster, "Miss", Color.red));

            return feedbacks;
        }
    }
}