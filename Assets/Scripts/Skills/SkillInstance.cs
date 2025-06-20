using Game.UI;
using System.Collections;
using Core;
using UnityEngine;

namespace Game.Skills {
    /// <summary>
    /// SkillInstance привязывает базовый Skill к конкретному владельцу (юниту),
    /// хранит текущий уровень, XP и знает — прокачиваемый это навык или фиксированный (champion).
    /// </summary>
    [System.Serializable]
    public class SkillInstance {
        public Skill baseSkill;
        public SkillLevelData levelData;
        public bool isChampionOwned;

        public SkillInstance(Skill skill, bool isChampion = false) {
            baseSkill = skill;
            isChampionOwned = isChampion;
            levelData = new SkillLevelData();

            if (isChampion) {
                // опционально: можно задать фиксированный уровень для чемпиона
                levelData = new SkillLevelData();
                for (int i = 1; i < skill.initialSkillLevel; i++)
                    levelData.TryGainXp(); // или задать через Init(...)
            }
        }

        // SkillInstance.cs
        public bool TryApply(UnitBase caster, UnitBase target) {
            caster.PlaySkillAnimation(baseSkill.animationType);

            float chance = levelData.SuccessChance;
            if (Random.value > chance) {
                FloatingTextSpawner.Instance.Spawn("Miss", caster.transform.position + Vector3.up * 1.5f, Color.red, false);
                Debug.Log($"[{baseSkill.name}] failed to apply (chance {chance * 100:0.0}%)");
                return false;
            }

            caster.StartCoroutine(DelayedSkillUse(0.5f, baseSkill, caster, target, levelData.PowerMultiplier));

            if (!isChampionOwned)
                levelData.TryGainXp();

            return true;
        }


        public string GetDisplayName() {
            return $"{baseSkill.skillName} (Lv. {levelData.Level})";
        }

        IEnumerator DelayedSkillUse(float delay, Skill skill, UnitBase caster, UnitBase target, float multiplier) {
            yield return new WaitForSeconds(delay);
            skill.Apply(caster, target, multiplier);
        }

    }
}