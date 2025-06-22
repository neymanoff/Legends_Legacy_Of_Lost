using System;
using Game.UI;
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
                // Промах — только “Miss” над кастером
                FloatingTextSpawner.Instance.Spawn(
                    "Miss",
                    caster.transform.position + Vector3.up * 1.5f,
                    Color.red
                );

                RemainingCooldown = Definition.Cooldown;
                return false;
            }

            // Успех — само применение и спавн урона/фидбэков
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