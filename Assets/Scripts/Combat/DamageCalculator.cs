using Core;
using UnityEngine;

namespace Game.Combat {
    public static class DamageCalculator {
        public static int Calculate(
            UnitBase caster,
            UnitBase target,
            int baseAmount,
            bool isMagic,
            float critChance,
            float critMultiplier) {
            float attack = isMagic ? caster.stats.intelligence : caster.stats.attackPower;
            float defense = isMagic ? target.stats.spirit : target.stats.defense;

            float multiplier = 1f + (attack - defense) / 100f;
            float dmg = baseAmount * multiplier;



            float totalCritChance = critChance + caster.stats.critChance;
            if (Random.value < Mathf.Clamp01(totalCritChance))
                dmg *= critMultiplier;

            dmg *= Random.Range(0.85f, 1.20f);
            return Mathf.Max(Mathf.RoundToInt(dmg), 0);
        }
    }
}