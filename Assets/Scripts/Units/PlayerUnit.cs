using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Units
{
    public class PlayerUnit : UnitBase {
        [Header("Player Skills")]
        
        //for SkillButtonsUI
        public int SkillCount => skills.Count;
        public string GetSkillDisplayName(int i) => skills[i].SkillName;


        public override void UseSkill(UnitBase target) {
            if (SelectedSkill == null || !target.isAlive) return;
            PlaySkillAnimation(SelectedSkill.AnimationType);
            SelectedSkill.Apply(this, target, 1f);
            SelectedSkill = null;
        }

        public void SetSelectedSkill(int index)
        {
            if (index >= 0 && index < skills.Count)
                SelectedSkill = skills[index];
        }

        public void UseSkillFromUI(int index, UnitBase target)
        {
            if (index < 0 || index >= skills.Count)
            {
                Debug.LogWarning($"Invalid skill index: {index}");
                return;
            }
            
            SelectedSkill = skills[index];
            base.TakeTurn(target);
        }
        
        public string GetSkillName(int index)
        {
            if (index < 0 || index >= skills.Count) return "No skill";
            return skills[index].SkillName;
        }

        protected override Skill[] GetSkills() => skills.ToArray();

    }
}
