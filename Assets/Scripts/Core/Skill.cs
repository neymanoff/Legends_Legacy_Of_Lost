using Core;
using UnityEngine;

namespace Game.Skills {
    public abstract class Skill : ScriptableObject {
        public string skillName;
        public int baseAmount;
        public Sprite icon;
        public int initialSkillLevel = 1;
        public SkillAnimationType animationType;


        public abstract void Apply(UnitBase caster, UnitBase target, float powerMultiplier);
    }
}
