// Copyright (c) Night Gamer. All rights reserved.
// ==========================
// GameplayEffect.cs
// 类似 UE 中的 UGameplayEffect，用于描述技能造成的效果
// 例如伤害、治疗、Buff 等，这里以伤害为例
// ==========================

public abstract class GameplayEffect
{
    public string Name { get; private set; }

    protected GameplayEffect(string name)
    {
        Name = name;
        Console.WriteLine($"[GE] 创建技能效果 {name}");
    }

    public abstract void Apply(AbilityContext context, EffectHandler handler);
}