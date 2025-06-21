using Core;
using UnityEngine;

namespace Units
{
    /// <summary>
    /// AI-controlled unit. Uses the first skill automatically.
    /// </summary>
    public class EnemyUnit : UnitBase {
        [SerializeField] private Skill defaultSkill;

        public override void UseSkill(UnitBase target) {
            var skill = skills.Count > 0 ? skills[0] : null;
            if (skill == null)
            {
                Debug.LogWarning($"{name} has no defaultSkill!");
                return;
            }
            PlaySkillAnimation(skill.AnimationType);
            skill.Apply(this, target, 1f);
        }
    }
}
