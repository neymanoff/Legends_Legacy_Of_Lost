using System;
using Game.UI;
using UnityEngine;
using Units; // для проверки типа юнита

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
            LevelData = new SkillLevelData();
            for (int i = 0; i < initialLevel; i++)
                LevelData.TryGainXp();
            RemainingCooldown = 0;
        }

        public bool TryApply(UnitBase caster, UnitBase target)
        {
            if (!IsReady) return false;
            caster.PlaySkillAnimation(Definition.AnimationType);

            float chance = LevelData.SuccessChance;
            if (UnityEngine.Random.value > chance)
            {
                FloatingTextSpawner.Instance.Spawn("Miss", caster.transform.position + Vector3.up * 1.5f, Color.red, false);
                RemainingCooldown = Definition.Cooldown;
                return false;
            }

            Definition.Apply(caster, target, LevelData.PowerMultiplier);
            RemainingCooldown = Definition.Cooldown;

            // Только герои получают XP и прокачивают навык
            if (caster is PlayerUnit)
            {
                LevelData.TryGainXp();
            }

            return true;
        }

        public void TickCooldown()
        {
            if (RemainingCooldown > 0)
                RemainingCooldown--;
        }
    }
}