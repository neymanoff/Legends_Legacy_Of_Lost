
using Core;

namespace Game.Combat {
    public static class SkillUtility {
        public static bool DidHit(UnitBase caster, UnitBase target, float baseHitChance = 0.95f) {
            float roll = UnityEngine.Random.value;
            return roll <= baseHitChance;
        }
    }
}
