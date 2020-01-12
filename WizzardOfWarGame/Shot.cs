using System;
using Newtonsoft.Json;

namespace wow.Core.Extentions.WizzardOfWarGame
{
    public class Shot : NonPlayerEnity
    {
        [JsonIgnore]
        public Player Owner;

        public Shot()
        {
            Type = EntitieType.Shot;
        }


        public override void Collision(Entity opponent)
        {
            // we are now dead
            Game.Kill(this.Id);

            // we hit something
            if(opponent is IWorthPoints)
            {
                var amount = (opponent as IWorthPoints).WorthPoints;
                Owner.AddPoints(amount);
            }
        }

        public override void HitWall(Direction direction)
        {
            // we just died
            Game.Kill(this.Id);
            Console.WriteLine("Hit the wall");
        }

        public override void Update()
        {
            // move forward
            Game.MoveEnity(this.Id,this.Direction);
            Console.WriteLine("moved shot to" + JsonConvert.SerializeObject(this.Position) + this.Direction);
        }
    }
}