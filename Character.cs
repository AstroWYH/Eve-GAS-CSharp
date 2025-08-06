// Copyright (c) Night Gamer. All rights reserved.
// ==========================
// Character.cs
// 代表技能释放者（角色），持有ASC和AttributeSet
// 类似UE中的ACharacter或APlayerState
// ==========================

public class Character : Actor
{
    public AbilitySystemComponent ASC { get; private set; }

    public Character(string name, EffectHandler handler) : base(name)
    {
        ASC = new AbilitySystemComponent(handler, this); // 传入自身到ASC
    }
}