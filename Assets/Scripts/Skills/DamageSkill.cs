using Core;
using Game.Combat;
using Game.Skills;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(menuName = "Skills/Physical")]
    public class DamageSkill : Skill {
        public float critChance = 0.1f;
        public float critMultiplier = 2f;

        public override void Apply(UnitBase caster, UnitBase target, float powerMultiplier) {
            int dmg = DamageCalculator.Calculate(
                caster,
                target,
                Mathf.RoundToInt(baseAmount * powerMultiplier),
                isMagic: false, // ← физическая атака
                critChance,
                critMultiplier
            );
        
            target.TakeDamage(dmg);
            Debug.Log($"{caster.name} strikes {target.name} with {name} for {dmg} physical dmg");
        }
    }
}
