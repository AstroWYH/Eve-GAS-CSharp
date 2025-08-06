// Copyright (c) Night Gamer. All rights reserved.
// ==========================
// AbilityContext.cs
// 类似 UE 中的 FGameplayAbilityActorInfo 或 FGameplayEffectContextHandle
// 用于在技能执行过程中传递施法者、目标、伤害数值等上下文信息
// ==========================

public class AbilityContext
{
    public Character Source { get; private set; }
    public Target Target { get; private set; }
    public float Damage { get; set; }

    public AbilityContext(Character source, Target target, float damage = 0)
    {
        Source = source;
        Target = target;
        Damage = damage;
    }
}