using System;
using Newtonsoft.Json;

namespace wow.Core.Extentions.WizzardOfWarGame
{
    public class Enemy : NonPlayerEnity
    {
        public Enemy()
        {
            Type = EntitieType.Enemy1;
            TicksBetweenUpdate = 30;
        }

        public override void Collision(Entity opponent)
        {
            if(opponent is Shot)
            {
                Game.Kill(this.Id);
                return;
            }
            Console.WriteLine($"enemy collided with {opponent.GetType().Name}");
        }

        public override void HitWall(Direction direction)
        {
            // change direction
            var newDirection = Game.GetFreeDirection(this.Position,direction);

            if(newDirection == Direction.None)
            {
                // We can't move, just die
                Console.WriteLine("dang it can't move");
                return;
            }
            this.Direction = newDirection;

            Console.WriteLine($"Enemy hit wall at {JsonConvert.SerializeObject(this.Position)} heading {this.Direction} coming from {direction}");
            

            // execute the move in the new direction
            Game.MoveEnity(this.Id,this.Direction);
            
        }

        public override void Update()
        {
            if(Game.random.Next(0,3) == 0)
            {
                // try to change direction
                HitWall(Direction.None);
                return;
            }
            // move forwar
            Game.MoveEnity(this.Id,this.Direction);
        }
    }
}