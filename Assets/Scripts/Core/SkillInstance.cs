using System;
using UI.Battle;
using UnityEngine;
using Units; 

namespace Core
{
    [Serializable]
    public class SkillInstance
    {
        public Skill Definition { get; }
        public SkillLevelData LevelData { get; }
        public int RemainingCooldown { get; private set; }

        public bool IsReady => RemainingCooldown <= 0;

        public SkillInstance(Skill definition, int initialLevel = 1)
        {
            Definition = definition;
            LevelData = new SkillLevelData(initialLevel);
            RemainingCooldown = 0;
        }

        public bool TryApply(UnitBase caster, UnitBase target)
        {
            if (!IsReady) return false;

            caster.PlaySkillAnimation(Definition.AnimationType);

            float chance = LevelData.SuccessChance;
            if (UnityEngine.Random.value > chance)
            {
                FloatingTextSpawner.Instance.Spawn(
                    "Miss",
                    caster.transform.position + Vector3.up * 1.5f,
                    Color.red
                );
                RemainingCooldown = Definition.Cooldown;
                return false;
            }

            var feedbacks = Definition.Apply(caster, target, LevelData.PowerMultiplier);
            foreach (var fb in feedbacks)
            {
                FloatingTextSpawner.Instance.Spawn(
                    fb.Text,
                    fb.Target.transform.position + Vector3.up * 1.5f,
                    fb.Color
                );
            }
            RemainingCooldown = Definition.Cooldown;
            if (caster is PlayerUnit) LevelData.TryGainXp();
            return true;
        }


        public void TickCooldown()
        {
            if (RemainingCooldown > 0)
                RemainingCooldown--;
        }
    }
}