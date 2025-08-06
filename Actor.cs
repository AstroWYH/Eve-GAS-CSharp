// Copyright (c) Night Gamer. All rights reserved.
// ==========================
// Hero.cs
// 代表技能释放者（角色），持有ASC和AttributeSet
// 类似UE中的ACharacter或APlayerState
// ==========================

public class Actor
{
    public string Name { get; private set; }
    public AttributeSet AttributeSet { get; private set; } // 新增属性集

    public Actor(string name)
    {
        Name = name;
        AttributeSet = new AttributeSet(this);
    }
}