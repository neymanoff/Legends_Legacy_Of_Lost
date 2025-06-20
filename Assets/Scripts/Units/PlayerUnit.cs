using Core;
using Game.Skills;
using UnityEngine;

public class PlayerUnit : UnitBase {


    public override void UseSkill(UnitBase target) {
        if (SelectedSkill == null || !target.isAlive) return;
        PlaySkillAnimation(SelectedSkill.animationType);
        SelectedSkill.Apply(this, target, 1f);
        SelectedSkill = null;
    }

    public void SetSelectedSkill(int index)
    {
        if (index >= 0 && index < skills.Length)
            SelectedSkill = skills[index];
    }
}
