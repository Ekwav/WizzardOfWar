using System;

namespace wow.Core.Extentions.WizzardOfWarGame
{
    public class Shot : NonPlayerEnity
    {
        Player Owner;

        public override void Die(Entity opponent)
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

        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}