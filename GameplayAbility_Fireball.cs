// Copyright (c) Night Gamer. All rights reserved.
// ==========================
// GameplayAbility_Fireball.cs
// 继承自 GameplayAbilityBase，代表一个具体技能：火球术
// 类似 UE 中具体的技能 Blueprint 类（继承自UGameplayAbility）
// ==========================

public class GameplayAbility_Fireball : GameplayAbility
{
    public GameplayAbility_Fireball(string name, int cooldown, int manaCost) : base(name, cooldown, manaCost) { }

    protected override void ExecuteAbility(AbilityContext context, AbilitySystemComponent asc)
    {
        // 传递基础伤害到效果
        var damageEffect = new GameplayEffect_Damage("GE伤害效果", 20);
        asc.ApplyGameplayEffectToTarget(damageEffect, context); // 改为对目标应用效果
    }
}