using System;
using System.Collections.Generic;

namespace RelEcs
{
    internal class Trigger<T>
    {
        internal T Value = default!;
    }

    internal class SystemList
    {
        public readonly List<Type> List;
        public SystemList() => List = ListPool<Type>.Get();
    }

    internal class LifeTime
    {
        public int Value;
    }

    internal class TriggerLifeTimeSystem : ISystem
    {
        public void Run(World world)
        {
            var query = world.Query<Entity, SystemList, LifeTime>().Build();
            foreach (var (entity, systemList, lifeTime) in query)
            {
                lifeTime.Value++;

                if (lifeTime.Value < 2) return;

                ListPool<Type>.Add(systemList.List);
                world.Despawn(entity);
            }
        }
    }
}