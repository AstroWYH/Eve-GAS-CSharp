// Copyright (c) Night Gamer. All rights reserved.
// ==========================
// AttributeSet.cs
// ����UE�е�UAttributeSet
// ���ڹ����ɫ�ĸ������ԣ�����ֵ������ֵ�ȣ�
// �ṩ�����޸ĺͻ�ȡ�Ļ�������
// ==========================

using System;
using System.Collections.Generic;
using System.Net;
using System.Numerics;

public class AttributeSet
{
    // ������������ö�٣�����GAS�����Է���
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
    public object Owner { get; private set; } // ����������

    public AttributeSet(object owner)
    {
        Owner = owner;
    }

    public void InitAttributeSet(float maxHealth, float health, float maxMana, float Mana, float attackPower, float defense)
    {
        // ��ʼ�����Բ���¼��־
        SetAttribute(AttributeType.MaxHealth, maxHealth, Owner);
        SetAttribute(AttributeType.Health, health, Owner);
        SetAttribute(AttributeType.MaxMana, maxMana, Owner);
        SetAttribute(AttributeType.Mana, Mana, Owner);
        SetAttribute(AttributeType.AttackPower, attackPower, Owner);
        SetAttribute(AttributeType.Defense, defense, Owner);
    }

    // �������ߺ���־����������
    public void SetAttribute(AttributeType type, float value, object instigator)
    {
        if (attributes.TryGetValue(type, out float oldValue))
        {
            if (Math.Abs(oldValue - value) < 0.001f) return; // ���⸡������������

            attributes[type] = value;
            OnAttributeChanged?.Invoke(type, oldValue, value, instigator);
            if (instigator is Actor ins && Owner is Actor owner)
                Console.WriteLine($"[AS] {owner.Name} �޸����� {type}: {oldValue:F1} �� {value:F1}��������: {ins.Name}");
        }
        else
        {
            attributes.Add(type, value);
            OnAttributeChanged?.Invoke(type, 0, value, instigator);
            if (instigator is Actor ins && Owner is Actor owner)
                Console.WriteLine($"[AS] {owner.Name} ��ʼ������ {type}: {oldValue:F1} �� {value:F1}��������: {ins.Name}");
        }
    }

    public float GetAttribute(AttributeType type)
    {
        attributes.TryGetValue(type, out float value);
        return value;
    }

    // ��Լ���������޸ģ�������ֵ���ܳ������ֵ��
    public void ModifyAttribute(AttributeType type, float delta, object instigator)
    {
        float current = GetAttribute(type);
        float newValue = current + delta;

        // ��������Լ��������GAS�����Թ���
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