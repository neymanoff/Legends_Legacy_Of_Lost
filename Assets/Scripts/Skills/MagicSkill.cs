using Core;
using UnityEngine;
using Game.Combat;

namespace Game.Skills {
    [CreateAssetMenu(menuName = "Skills/Magic")]
    public class MagicSkill : Skill {
        public float critChance = 0.15f;
        public float critMultiplier = 2f;

        public override void Apply(UnitBase caster, UnitBase target, float powerMultiplier)
        {
            int dmg = DamageCalculator.Calculate(
                caster,
                target,
                Mathf.RoundToInt(baseAmount * powerMultiplier),
                isMagic: true,
                critChance,
                critMultiplier
            );

            target.TakeDamage(dmg);
            Debug.Log($"{caster.name} casts {name} on {target.name} for {dmg} magic dmg");
        }
    }
}