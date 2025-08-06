// Copyright (c) Night Gamer. All rights reserved.
// ==========================
// AttributeSet.cs
// 类似UE中的UAttributeSet
// 用于管理角色的各种属性（生命值、法力值等）
// 提供属性修改和获取的基础功能
// ==========================

using System;
using System.Collections.Generic;
using System.Net;
using System.Numerics;

public class AttributeSet
{
    // 定义属性类型枚举，体现GAS的属性分类
    public enum AttributeType
    {
        Health,
        MaxHealth,
        Mana,
        MaxMana,
        AttackPower,
        Defense
    }

    private Dictionary<AttributeType, float> attributes = new();
    public event Action<AttributeType, float, float, object> OnAttributeChanged;
    public object Owner { get; private set; } // 持有所有者

    public AttributeSet(object owner)
    {
        Owner = owner;
    }

    public void InitAttributeSet(float maxHealth, float health, float maxMana, float Mana, float attackPower, float defense)
    {
        // 初始化属性并记录日志
        SetAttribute(AttributeType.MaxHealth, maxHealth, Owner);
        SetAttribute(AttributeType.Health, health, Owner);
        SetAttribute(AttributeType.MaxMana, maxMana, Owner);
        SetAttribute(AttributeType.Mana, Mana, Owner);
        SetAttribute(AttributeType.AttackPower, attackPower, Owner);
        SetAttribute(AttributeType.Defense, defense, Owner);
    }

    // 带发起者和日志的属性设置
    public void SetAttribute(AttributeType type, float value, object instigator)
    {
        if (attributes.TryGetValue(type, out float oldValue))
        {
            if (Math.Abs(oldValue - value) < 0.001f) return; // 避免浮点数精度问题

            attributes[type] = value;
            OnAttributeChanged?.Invoke(type, oldValue, value, instigator);
            if (instigator is Actor ins && Owner is Actor owner)
                Console.WriteLine($"[AS] {owner.Name} 修改属性 {type}: {oldValue:F1} → {value:F1}，发起者: {ins.Name}");
        }
        else
        {
            attributes.Add(type, value);
            OnAttributeChanged?.Invoke(type, 0, value, instigator);
            if (instigator is Actor ins && Owner is Actor owner)
                Console.WriteLine($"[AS] {owner.Name} 初始化属性 {type}: {oldValue:F1} → {value:F1}，发起者: {ins.Name}");
        }
    }

    public float GetAttribute(AttributeType type)
    {
        attributes.TryGetValue(type, out float value);
        return value;
    }

    // 带约束的属性修改（如生命值不能超过最大值）
    public void ModifyAttribute(AttributeType type, float delta, object instigator)
    {
        float current = GetAttribute(type);
        float newValue = current + delta;

        // 特殊属性约束（体现GAS的属性规则）
        if (type == AttributeType.Health)
        {
            newValue = Math.Clamp(newValue, 0, GetAttribute(AttributeType.MaxHealth));
        }
        else if (type == AttributeType.Mana)
        {
            newValue = Math.Clamp(newValue, 0, GetAttribute(AttributeType.MaxMana));
        }

        SetAttribute(type, newValue, instigator);
    }
}