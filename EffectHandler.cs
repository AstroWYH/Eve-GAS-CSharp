// Copyright (c) Night Gamer. All rights reserved.
// ==========================
// EffectHandler.cs
// 类似 UE 中的 GameplayCueManager / CueNotifyActor
// 用于处理实际的表现逻辑（如播放特效、广播伤害）
// 也包含一个简单的委托机制，模拟事件通知
// ==========================

using System;

public class EffectHandler
{
    public delegate void DamageEvent(AbilityContext context);
    public event DamageEvent OnDamage;

    public void ApplyEffect(AbilityContext context)
    {
        // 打印效果表现日志（类似UE的GameplayCue）
        Console.WriteLine($"[Effect] 特效：{context.Source.Name} 对 {context.Target.Name} 造成 {context.Damage} 点伤害（火花四溅！）");

        OnDamage?.Invoke(context);
    }
}