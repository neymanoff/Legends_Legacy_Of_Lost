using Core;
using Game.Skills;
using UnityEngine;

/// <summary>
/// AI-controlled unit. Uses the first skill automatically.
/// </summary>
public class EnemyUnit : UnitBase {
    [SerializeField] private Skill defaultSkill;

    public override void UseSkill(UnitBase target) {
        var skill = skills.Length > 0 ? skills[0] : null;
        if (skill != null) {
            PlaySkillAnimation(skill.animationType);
            skill.Apply(this, target, 1f);
        }
    }
}
