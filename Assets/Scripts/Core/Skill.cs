using System.Collections.Generic;
using UnityEngine;


public enum SkillAnimationType
{
    None,
    Melee,
    Ranged,
    Magic,
    Block
}

namespace Core
{
    [CreateAssetMenu(fileName = "New Skill", menuName = "Skills/Skill")]
    public abstract class Skill : ScriptableObject
    {
        [Header("Basic Info")] 
        [SerializeField]private string skillName;
        [SerializeField]private Sprite icon;
        [TextArea]
        [SerializeField] private string description;
        
        [Header("Effect Parameters")]
        [SerializeField]private int baseAmount;
        [SerializeField]private SkillAnimationType animationType;
        [SerializeField] private int cooldownTurns;
        
        public string SkillName => skillName;
        public Sprite Icon => icon;
        public string Description => description;
        public int BaseAmount => baseAmount;
        public SkillAnimationType AnimationType => animationType;
        public int Cooldown => cooldownTurns;


        public abstract List<SkillFeedback> Apply(UnitBase caster, UnitBase target, float powerMultiplier);
    }
}