using System;
using System.Runtime.Serialization;
using Coflnet;
using MessagePack;
using Newtonsoft.Json;

namespace wow.Core.Extentions.WizzardOfWarGame
{
    
    public abstract class Entity
    {
        public virtual SourceReference Id{get;set;}

        public EntitieType Type;

        public Position Position;

        public Direction Direction;

        [JsonIgnore]
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

        public abstract void Collision(Entity opponent);

        /// <summary>
        /// A move hit a wall
        /// </summary>
        /// <param name="direction">The direction that was being tried to move in</param>
        public abstract void HitWall(Direction direction);
    }
}