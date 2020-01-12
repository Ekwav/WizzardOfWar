using System;
using Coflnet;
using MessagePack;

namespace wow.Core.Extentions.WizzardOfWarGame
{
    [MessagePackObject]
    public abstract class Entity
    {
        public SourceReference Id;

        public EntitieType Type;

        public Position Position;

        public Direction Direction;

        public Game Game;

        public bool IsDead;

        public enum EntitieType {
            Unknown,
            Player,
            Shot,
            Enemy1 = 32,
            Enemy2 = 33,
            Enemy3 = 34,
            Enemy4 = 35
        }

        public abstract void Tick();

        public abstract void Die(Entity opponent);

        /// <summary>
        /// A move hit a wall
        /// </summary>
        /// <param name="direction">The direction that was being tried to move in</param>
        public abstract void HitWall(Direction direction);
    }
}