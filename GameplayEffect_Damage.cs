// Copyright (c) Night Gamer. All rights reserved.
// ==========================
// GameplayEffect_Damage.cs
// 一个具体的GameplayEffect实现，造成伤害
// 现在使用AttributeSet来修改目标生命值
// ==========================

public class GameplayEffect_Damage : GameplayEffect
{
    private int BaseDamage;

    public GameplayEffect_Damage(string name, int baseDamage) : base(name)
    {
        BaseDamage = baseDamage;
    }

    public override void Apply(AbilityContext context, EffectHandler handler)
    {
        // 从属性集获取攻击者攻击力和目标防御力（体现GAS的属性联动）
        float attackPower = context.Source.AttributeSet.GetAttribute(AttributeSet.AttributeType.AttackPower);
        float targetDefense = context.Target.AttributeSet.GetAttribute(AttributeSet.AttributeType.Defense);

        // 伤害公式：基础伤害 + 攻击力 - 目标防御（最低1点伤害）
        float finalDamage = Math.Max(1, BaseDamage + attackPower - targetDefense);
        context.Damage = finalDamage;
        // 应用伤害到目标生命值
        context.Target.AttributeSet.ModifyAttribute(
            AttributeSet.AttributeType.Health,
            -finalDamage,
            context.Source);

        // 通知伤害事件
        handler.ApplyEffect(context);
    }
}