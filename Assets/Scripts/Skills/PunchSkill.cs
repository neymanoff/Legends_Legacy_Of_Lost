using Core;
using UnityEngine;
using Game.Combat;
using Game.Skills;
using Game.UI;

[CreateAssetMenu(menuName = "Skills/Punch")]
public class PunchSkill : Skill {
    [Header("Skill Settings")]
    public float critMultiplier = 1.5f;
    public float critChance = 0.15f;
    public float hitChance = 0.95f;

    public override void Apply(UnitBase caster, UnitBase target, float multiplier) {
        if (!SkillUtility.DidHit(caster, target, hitChance)) {
            FloatingTextSpawner.Instance?.Spawn("Miss", caster.transform.position + Vector3.up * 2f, Color.white, false);
            return;
        }

        int dmg = Mathf.RoundToInt(baseAmount * multiplier) + caster.stats.attackPower;

        bool isCrit = Random.value < (critChance + caster.stats.critChance);
        if (isCrit)
            dmg = Mathf.RoundToInt(dmg * critMultiplier);

        target.TakeDamage(dmg);

        FloatingTextSpawner.Instance?.Spawn(dmg.ToString(), target.transform.position + Vector3.up * 2f, Color.red, isCrit);
    }
}
