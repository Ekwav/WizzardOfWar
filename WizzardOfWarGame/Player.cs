using Coflnet;

namespace wow.Core.Extentions.WizzardOfWarGame
{
    public interface IWorthPoints {
        int WorthPoints {get;}
    }

    public class Player : Entity,IWorthPoints
    {
        public WoWProxy Connection;

        public long Points;

        public int WorthPoints => 1000;


        public override void Die(Entity opponent)
        {
            Game.SendCommand(ProxyMessageData.Create("respawn",new RespawnCommandParams(this.Id,Game.GetRespawnPos(),3)));
            // dang it, we are dead
            // Todo: count up deaths to perma death on 3
        }

        public void AddPoints(int amount)
        {
            Points += amount;
            Game.SendCommand(ProxyMessageData.Create("setPoints",new PointCommandParams(this.Id,Points)));
        }

        public override void HitWall(Direction direction)
        {
            // nothgin to do really other than counting up funny stats (Hit my head x times)
        }

        public override void Tick()
        {
            // nothing to do here currently
        }

        public void SendCommand(ProxyMessageData data)
        {
            Connection.SendBack(data);
        }

        class PointCommandParams : IdContainer
        {
            public long Points;

            public PointCommandParams(SourceReference id, long points) : base(id)
            {
                Points = points;
            }
        }

        class RespawnCommandParams : IdContainer
        {
            public Position position;
            public long Delay;

            public RespawnCommandParams(SourceReference id, Position position, long delay) : base(id)
            {
                this.position = position;
                Delay = delay;
            }
        }
    }
}