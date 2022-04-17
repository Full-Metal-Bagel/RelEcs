using System;
using System.Collections.Generic;

namespace Bitron.Ecs
{
    internal sealed class Mask
    {
        internal List<TypeId> TargetRelations { get; private set; } = new List<TypeId>();
        internal List<TypeId> SourceRelations { get; private set; } = new List<TypeId>();
        internal List<int> AnyRelations { get; private set; } = new List<int>();

        internal BitSet IncludeBitSet = new BitSet();
        internal BitSet ExcludeBitSet = new BitSet();


        // BitSet addedBitSet = new BitSet();
        // BitSet removedBitSet = new BitSet();

        internal void With<T>(Entity target) where T : struct
        {
            var typeId = TypeId.Get<T>(target.Id);
            IncludeBitSet.Set(typeId.Index);
        }

        internal void Without<T>(Entity target) where T : struct
        {
            var typeId = TypeId.Get<T>(target.Id);
            ExcludeBitSet.Set(typeId.Index);
        }

        // internal void Without<T>(Entity source, Entity target) where T : struct
        // {
        //     var typeId = TypeId.Get<T>(target.Id);
        //     ExcludeBitSet.Set(typeId.Index);
        // }

        // internal void Added<T>() where T : struct
        // {
        //     var typeId = ComponentType<T>.Id;
        //     addedBitSet.Set(typeId);
        // }

        // internal void Removed<T>() where T : struct
        // {
        //     var typeId = ComponentType<T>.Id;
        //     removedBitSet.Set(typeId);
        // }
    }
}