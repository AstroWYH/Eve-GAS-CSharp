// Copyright (c) Night Gamer. All rights reserved.
// ==========================
// GameplayAbility.cs
// 类比于 UE 中的 UGameplayAbility，是技能的基类
// 负责控制技能冷却判断、激活流程、调用执行逻辑
// 可继承实现具体技能逻辑，例如火球术等
// ==========================

using System;

public abstract class GameplayAbility
{
    public string Name { get; private set; }
    public int CooldownSeconds { get; private set; }
    public int ManaCost { get; private set; } // 技能消耗的法力值
    private DateTime lastUsed = DateTime.MinValue;

    // 构造函数新增法力值消耗参数
    public GameplayAbility(string name, int cooldown, int manaCost)
    {
        Name = name;
        CooldownSeconds = cooldown;
        ManaCost = manaCost;
    }

    // 检查是否可激活（结合冷却和资源）
    public bool CanActivate(AttributeSet attributeSet)
    {
        var remainingCooldown = CooldownSeconds - (DateTime.Now - lastUsed).TotalSeconds;
        if (remainingCooldown > 0)
        {
            Console.WriteLine($"[GA] {Name} 冷却中，剩余 {remainingCooldown:F1} 秒");
            return false;
        }

        if (attributeSet.GetAttribute(AttributeSet.AttributeType.Mana) < ManaCost)
        {
            Console.WriteLine($"[GA] {Name} 法力值不足（当前: {attributeSet.GetAttribute(AttributeSet.AttributeType.Mana)}, 所需: {ManaCost}）");
            return false;
        }

        return true;
    }

    public void Activate(AbilityContext context, AbilitySystemComponent asc)
    {
        // 消耗法力值（体现GAS的资源系统）
        asc.AttributeSet.ModifyAttribute(AttributeSet.AttributeType.Mana, -ManaCost, asc.Owner);
        lastUsed = DateTime.Now;

        Console.WriteLine($"[GA] {asc.Owner.Name} 成功激活 {Name}，冷却 {CooldownSeconds} 秒");
        ExecuteAbility(context, asc);
    }

    protected abstract void ExecuteAbility(AbilityContext context, AbilitySystemComponent asc);
}