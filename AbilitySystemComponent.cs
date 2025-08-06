// Copyright (c) Night Gamer. All rights reserved.
// ==========================
// AbilitySystemComponent.cs
// 类似UE中的UAbilitySystemComponent（ASC）
// 是技能系统的核心组件，负责技能授权、激活与效果应用
// 每个角色Actor持有一个ASC，统一管理技能系统相关逻辑
// ==========================

using System;
using System.Collections.Generic;

public class AbilitySystemComponent
{
    private Dictionary<string, GameplayAbility> Abilities = new();
    private EffectHandler Handler;
    public Character Owner { get; private set; } // 持有所有者
    public AttributeSet AttributeSet => Owner.AttributeSet;

    public AbilitySystemComponent(EffectHandler handler, Character owner)
    {
        Handler = handler;
        Owner = owner;

        // 订阅属性变化，打印关键信息（如死亡）
        AttributeSet.OnAttributeChanged += (type, old, val, _) =>
        {
            if (type == AttributeSet.AttributeType.Health && val <= 0)
            {
                Console.WriteLine($"[ASC] {Owner.Name} 生命值归0，已死亡！");
            }
        };
    }

    public void GrantAbility(GameplayAbility ability)
    {
        if (Abilities.ContainsKey(ability.Name))
        {
            Console.WriteLine($"[ASC] {Owner.Name} 已拥有技能 {ability.Name}，无需重复授予");
            return;
        }
        Abilities[ability.Name] = ability;
        Console.WriteLine($"[ASC] 授予技能 {ability.Name} 给 {Owner.Name}");
    }

    public void TryActivateAbility(string abilityName, AbilityContext context)
    {
        if (!Abilities.TryGetValue(abilityName, out var ability))
        {
            Console.WriteLine($"[ASC] {Owner.Name} 未拥有技能 {abilityName}，无法激活");
            return;
        }

        // 检查技能是否可激活（封装在ASC中，体现GAS的集中控制）
        if (!ability.CanActivate(Owner.AttributeSet))
        {
            Console.WriteLine($"[ASC] {Owner.Name} 的技能 {abilityName} 无法激活（冷却中或资源不足）");
            return;
        }

        Console.WriteLine($"[ASC] {Owner.Name} 开始激活技能 {abilityName}");
        ability.Activate(context, this);
    }

    public void ApplyGameplayEffectToTarget(GameplayEffect effect, AbilityContext context)
    {
        Console.WriteLine($"[ASC] {Owner.Name} 对 {context.Target.Name} 应用效果 {effect.Name}");
        effect.Apply(context, Handler);
    }
}