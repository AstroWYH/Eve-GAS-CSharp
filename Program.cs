// Copyright (c) Night Gamer. All rights reserved.
// ==========================
// Program.cs
// 入口主程序，对应UE中 GameMode 或测试入口点
// 用于验证 GAS 系统运行效果，模拟施法、冷却等流程
// ==========================

using System;
using System.Threading;

using System;
using System.Threading;
using static AttributeSet;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== GAS系统测试开始 ===");
        var handler = new EffectHandler();
        handler.OnDamage += (context) =>
            Console.WriteLine($"[战斗日志] {context.Target.Name} 受到 {context.Source.Name} 的 {context.Damage} 点伤害，剩余生命值 {context.Target.AttributeSet.GetAttribute(AttributeType.Health)}");

        // 依赖注入EffectHandler
        var hero = new Character("英雄", handler);
        hero.AttributeSet.InitAttributeSet(100, 100, 50, 50, 20, 10);
        var goblin = new Target("哥布林");
        goblin.AttributeSet.InitAttributeSet(80, 80, 30, 30, 20, 10);

        // 授予技能 
        hero.ASC.GrantAbility(new GameplayAbility_Fireball("GA火球术", 3, 20));

        // 测试技能激活（第一次：正常激活）
        hero.ASC.TryActivateAbility("GA火球术", new AbilityContext(hero, goblin));
        Thread.Sleep(1000);

        // 测试冷却（1秒后再次激活，冷却未结束）
        hero.ASC.TryActivateAbility("GA火球术", new AbilityContext(hero, goblin));
        Thread.Sleep(3000); // 等待冷却结束

        // 测试法力值不足（多次释放后法力值耗尽）
        for (int i = 0; i < 6; i++)
        {
            hero.ASC.TryActivateAbility("GA火球术", new AbilityContext(hero, goblin));
            Thread.Sleep(1000);
        }

        Console.WriteLine("=== GAS系统测试结束 ===");
    }
}