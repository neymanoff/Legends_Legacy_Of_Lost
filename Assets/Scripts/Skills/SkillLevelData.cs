using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Skills {
    [System.Serializable]
    public class SkillLevelData {
        [SerializeField] private int level = 1;
        [FormerlySerializedAs("currentXP")] [SerializeField] private int currentXp = 0;

        public int Level => level;
        public int Xp => currentXp;

        public float SuccessChance => Mathf.Clamp01(level / 50f);
        public float PowerMultiplier => 1f + 0.25f * (level - 1);
        public int MaxLevel => 100;

        public int RequiredXp => Mathf.RoundToInt((5 + level * level) * GetXpScaleFactor());

        private float GetXpScaleFactor() {
            if (level <= 50) return 1f;
            if (level <= 100) return 1.5f;
            return 2f;
        }

        public bool TryGainXp() {
            if (level >= MaxLevel) return false;

            currentXp++;
            if (currentXp >= RequiredXp) {
                level++;
                currentXp = 0;
                Debug.Log($"[Skill] Level up! New level: {level}");
                return true;
            }
            return false;
        }
    }
}