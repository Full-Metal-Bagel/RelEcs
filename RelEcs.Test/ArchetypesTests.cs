using System;
using NUnit.Framework;

namespace RelEcs.Tests
{
    [TestFixture]
    public class ArchetypesTests
    {
        private Archetypes _archetypes = default!;

        [SetUp]
        public void Setup()
        {
            _archetypes = new Archetypes();
        }

        [Test]
        public void Constructor_InitializesProperly()
        {
            Assert.IsNotNull(_archetypes);
            Assert.That(_archetypes.EntityCount, Is.Zero); // Assuming EntityCount starts at 0
        }

        [Test]
        public void Spawn_ReturnsValidEntity()
        {
            var entity = _archetypes.Spawn();
            Assert.That(entity, Is.Not.Null);
            Assert.That(_archetypes.IsAlive(entity.Identity), Is.True);
        }

        [Test]
        public void Despawn_RemovesEntity()
        {
            var entity = _archetypes.Spawn();
            _archetypes.Despawn(entity.Identity);
            Assert.That(_archetypes.IsAlive(entity.Identity), Is.False);
        }

        [Test]
        public void AddComponent_AddsComponentToEntity()
        {
            var entity = _archetypes.Spawn();
            var type = StorageType.Create<object>(); // Assuming a valid StorageType instance
            _archetypes.AddComponent(type, entity.Identity, new object());
            Assert.That(_archetypes.HasComponent(type, entity.Identity), Is.True);
        }

        [Test]
        public void GetComponent_ReturnsValidComponent()
        {
            var entity = _archetypes.Spawn();
            var type = StorageType.Create<object>(); // Assuming a valid StorageType instance
            var data = new object();
            _archetypes.AddComponent(type, entity.Identity, data);
            var component = _archetypes.GetComponent(type, entity.Identity);
            Assert.That(component, Is.EqualTo(data));
        }

        [Test]
        public void Lock_LocksArchetypes()
        {
            _archetypes.Lock();
            var type = StorageType.Create<object>();
            var identify = _archetypes.Spawn().Identity;
            _archetypes.AddComponent(type, identify, new object());
            Assert.That(_archetypes.HasComponent(type, identify), Is.False);
            _archetypes.Unlock();
            Assert.That(_archetypes.HasComponent(type, identify), Is.True);
        }

        [Test]
        public void Unlock_UnlocksArchetypes()
        {
            var entity = _archetypes.Spawn();
            _archetypes.Lock();
            _archetypes.Unlock();
            Assert.DoesNotThrow(() => _archetypes.AddComponent(StorageType.Create<object>(), entity.Identity, new object())); // Assuming operations don't throw when unlocked
        }

        // ... Add more tests for edge cases, different scenarios, and possible exceptions ...
        [Test]
        public void AddComponent_ThrowsWhenComponentAlreadyExists()
        {
            var entity = _archetypes.Spawn();
            var type = StorageType.Create<object>(); // Assuming a valid StorageType instance
            _archetypes.AddComponent(type, entity.Identity, new object());
            Assert.Throws<Exception>(() =>
                _archetypes.AddComponent(type, entity.Identity,
                    new object())); // Assuming it throws an exception when trying to add an existing component
        }

        [Test]
        public void RemoveComponent_ThrowsWhenComponentDoesNotExist()
        {
            var entity = _archetypes.Spawn();
            var type = StorageType.Create<object>(); // Assuming a valid StorageType instance
            Assert.Throws<Exception>(() =>
                _archetypes.RemoveComponent(type,
                    entity.Identity)); // Assuming it throws an exception when trying to remove a non-existent component
        }

        [Test]
        public void GetComponent_ThrowsWhenComponentDoesNotExist()
        {
            var entity = _archetypes.Spawn();
            var type = StorageType.Create<object>(); // Assuming a valid StorageType instance
            Assert.Catch<Exception>(() =>
                _archetypes.GetComponent(type,
                    entity.Identity)); // Assuming it throws an exception when trying to get a non-existent component
        }
        //
        // [Test]
        // public void Despawn_ThrowsWhenEntityDoesNotExist()
        // {
        //     var identity = new Identity(); // Assuming a valid Identity instance not linked to any entity
        //     Assert.Throws<Exception>(() =>
        //         _archetypes.Despawn(
        //             identity)); // Assuming it throws an exception when trying to despawn a non-existent entity
        // }

        [Test]
        public void AddComponent_WhenLocked_QueuesOperation()
        {
            var entity = _archetypes.Spawn();
            var type = StorageType.Create<object>(); // Assuming a valid StorageType instance
            _archetypes.Lock();
            _archetypes.AddComponent(type, entity.Identity, new object());
            _archetypes.Unlock();
            Assert.IsTrue(_archetypes.HasComponent(type,
                entity.Identity)); // Assuming the component is added after unlocking
        }

        [Test]
        public void RemoveComponent_WhenLocked_QueuesOperation()
        {
            var entity = _archetypes.Spawn();
            var type = StorageType.Create<object>(); // Assuming a valid StorageType instance
            _archetypes.AddComponent(type, entity.Identity, new object());
            _archetypes.Lock();
            _archetypes.RemoveComponent(type, entity.Identity);
            _archetypes.Unlock();
            Assert.IsFalse(_archetypes.HasComponent(type,
                entity.Identity)); // Assuming the component is removed after unlocking
        }

        [Test]
        public void Spawn_WhenEntityLimitReached_ResizesMetaArray()
        {
            // Assuming the initial limit is 512 as per the provided code
            for (int i = 0; i < 512; i++)
            {
                _archetypes.Spawn();
            }

            Assert.DoesNotThrow(() =>
                _archetypes.Spawn()); // Assuming it doesn't throw an exception and resizes the Meta array
        }
    }
}