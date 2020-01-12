using System;
using Newtonsoft.Json;

namespace wow.Core.Extentions.WizzardOfWarGame
{
    public class Enemy : NonPlayerEnity
    {
        public Enemy()
        {
            Type = EntitieType.Enemy1;
            TicksBetweenUpdate = 100;
        }

        public override void Collision(Entity opponent)
        {
            if(opponent is Shot)
            {
                Game.Kill(this.Id);
                return;
            }
            Console.WriteLine("enemy collided");
        }

        public override void HitWall(Direction direction)
        {
            // change direction
            this.Direction = Game.GetFreeDirection(this.Position,direction);

            if(this.Direction == Direction.None)
            {
                // We can't move, just die
                Console.WriteLine("dang it can't move");
                return;
            }

            // execute the move in the new direction
            Game.MoveEnity(this.Id,this.Direction);
        }

        public override void Update()
        {
            // move forwar
            Game.MoveEnity(this.Id,this.Direction);
            Console.WriteLine("Enemy at "+ JsonConvert.SerializeObject(this.Position) + this.Direction);
        }
    }
}