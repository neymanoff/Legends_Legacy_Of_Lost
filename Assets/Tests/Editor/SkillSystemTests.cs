// Assets/Tests/Editor/SkillSystemTests.cs

using System;
using System.Collections.Generic;
using System.Reflection;
using Core;
using NUnit.Framework;
using UI.Battle;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tests.Editor
{
    public class SkillSystemTests
    {
        [SetUp]
        public void SetUpSpawner()
        {
            // Создаём спаунер и вручную вызываем его Awake(),
            // чтобы FloatingTextSpawner. Instance был установлен
            var go = new GameObject("FTS");
            var spawner = go.AddComponent<FloatingTextSpawner>();
            var awake = typeof(FloatingTextSpawner)
                .GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic);
            if (awake != null) awake.Invoke(spawner, null);
        }

        /// <summary> Устанавливает приватное (или унаследованное) поле через Reflection. </summary>
        private void SetPrivate<T>(object obj, string fieldName, T value)
        {
         var type = obj.GetType();
         FieldInfo field = null;
         while (type != null && field == null)
         {
             field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
              type = type.BaseType;
         }
         Assert.NotNull(field, $"Field '{fieldName}' not found on {obj.GetType().Name} or its base types");
         field.SetValue(obj, value);        
        }

        /// <summary>
        /// Мини-реализация Skill: урон = BaseAmount * multiplier,
        /// возвращает один SkillFeedback.
        /// </summary>
        private class DummySkill : Skill
        {
            public override List<SkillFeedback> Apply(UnitBase caster, UnitBase target, float powerMultiplier)
            {
                int dmg = Mathf.RoundToInt(BaseAmount * powerMultiplier);
                target.TakeDamage(dmg);
                return new List<SkillFeedback> {
                    dmg > 0
                        ? new SkillFeedback(target, dmg.ToString(), Color.white)
                        : new SkillFeedback(caster, "Miss", Color.red)
                };
        } 
        }

        /// <summary>
        /// «Лёгкая» заглушка UnitBase: пропускает Awake/OnValidate,
        /// не умирает при TakeDamage, и ничего не требует в GetSkills.
        /// </summary>
        private class TestUnit : UnitBase
        {
            protected override void Awake() { }
            #if UNITY_EDITOR
            protected override void OnValidate() { }
            #endif

            public override Skill[] GetSkills() => Array.Empty<Skill>();

            public override void TakeDamage(int amount)
            {
                if (amount > 0) Debug.Log($"{name} takes {amount} dmg (mock)");
                // не вызываем Die()
            }
        }

        [Test]
        public void DummySkill_Apply_ReturnsCorrectFeedback()
        {
            var skill = ScriptableObject.CreateInstance<DummySkill>();
            SetPrivate(skill, "baseAmount", 7);

            var caster = new GameObject().AddComponent<TestUnit>();
            var target = new GameObject().AddComponent<TestUnit>();

            var feedbacks = skill.Apply(caster, target, 2f);
            Debug.Log($"[DEBUG] feedbacks.Count = {feedbacks?.Count}");
            if (feedbacks is { Count: > 0 })
                Debug.Log($"[DEBUG] feedbacks[0]: Target={feedbacks[0].Target}, Text='{feedbacks[0].Text}', Color={feedbacks[0].Color}");
            if (feedbacks != null)
            {
                Assert.AreEqual(1, feedbacks.Count, "Один feedback");
                Assert.AreEqual("14", feedbacks[0].Text, "7 × 2 = 14");
                Assert.AreEqual(target, feedbacks[0].Target);
                Assert.AreEqual(Color.white, feedbacks[0].Color);
            }
        }

        [Test]
        public void TryApply_Hit_SetsCooldownAndReturnsTrue()
        {
            Random.InitState(42);
            var skill = ScriptableObject.CreateInstance<DummySkill>();
            SetPrivate(skill, "baseAmount",    5);
            SetPrivate(skill, "cooldownTurns", 2);

            var inst = new SkillInstance(skill, initialLevel: 50); // 100% hit
            var caster = new GameObject().AddComponent<TestUnit>();
            var target = new GameObject().AddComponent<TestUnit>();

            bool applied = inst.TryApply(caster, target);
            Assert.IsTrue(applied,                     "При 100% шансе → true");
            Assert.AreEqual(2, inst.RemainingCooldown, "RemainingCooldown == 2");
        }

        [Test]
        public void TryApply_Miss_SetsCooldownAndReturnsFalse()
        {
            Random.InitState(42);
            var skill = ScriptableObject.CreateInstance<DummySkill>();
            SetPrivate(skill, "baseAmount",    5);
            SetPrivate(skill, "cooldownTurns", 3);

            var inst = new SkillInstance(skill, initialLevel: 0);  // 0% hit
            var caster = new GameObject().AddComponent<TestUnit>();
            var target = new GameObject().AddComponent<TestUnit>();

            bool applied = inst.TryApply(caster, target);
            Assert.IsFalse(applied,                     "При 0% шансе → false");
            Assert.AreEqual(3, inst.RemainingCooldown,  "RemainingCooldown == 3");
        }

        [Test]
        public void EnemyUnit_InitialLevelRemainsStatic()
        {
            var skill = ScriptableObject.CreateInstance<DummySkill>();
            SetPrivate(skill, "baseAmount",    1);
            SetPrivate(skill, "cooldownTurns", 1);

            var inst = new SkillInstance(skill, initialLevel: 7);
            Assert.AreEqual(7, inst.LevelData.Level, "Начальный уровень == 7");

            var enemy  = new GameObject().AddComponent<TestUnit>();
            var victim = new GameObject().AddComponent<TestUnit>();
            inst.TryApply(enemy, victim);

            Assert.AreEqual(7, inst.LevelData.Level, "Level не меняется у не-PlayerUnit");
        }
    }
}
