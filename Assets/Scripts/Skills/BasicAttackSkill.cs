using Core;
using Game.UI;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Basic Attack", menuName = "Skills/Basic Attack")]
    public class BasicAttackSkill : Skill
    {
        public override void Apply(UnitBase caster, UnitBase target, float powerMultiplier)
        {
            int damage = Mathf.RoundToInt(BaseAmount * powerMultiplier);
            target.TakeDamage(damage);
            FloatingTextSpawner.Instance.Spawn(damage.ToString(), target.transform.position + Vector3.up * 1.5f, Color.white, true);
        }
    }
}