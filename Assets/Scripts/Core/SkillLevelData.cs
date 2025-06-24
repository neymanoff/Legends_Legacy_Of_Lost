using System;
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class SkillLevelData
    {
        [SerializeField] private int level = 1;
        [SerializeField] private int currentExp = 0;

        public int Level => level;
        public int CurrentExp => currentExp;
        
        

        //Success chance 100% only 50lv+
        public float SuccessChance => Mathf.Clamp01(level / 50f);

        //Power Multiplier +25% for each lvl 
        public float PowerMultiplier => 1f + 0.25f * (level - 1);

        // XP needed to LVLUp Skill
        public int RequiredExp => Mathf.RoundToInt((5 + level * level) * GetXpScaleFactor());

        private float GetXpScaleFactor()
        {
            if (level <= 50) return 1f;
            if (level <= 100) return 1.5f;
            return 2f;
        }

        public bool TryGainXp(int xpAmount = 1)
        {
            if (level >= 100) return false;

            currentExp += xpAmount;
            if (currentExp >= RequiredExp)
            {
                level++;
                currentExp = 0;
                return true;
            }

            return false;
            
        }
        
        public SkillLevelData(int initialLevel)
        {
            level = initialLevel;
            currentExp = 0;
        }
    }
}