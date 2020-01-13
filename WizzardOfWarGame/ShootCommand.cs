using System.Collections.Generic;
using Coflnet;

namespace wow.Core.Extentions.WizzardOfWarGame
{
    public class ShootCommand : Command
    {
        public override string Slug => "shoot";

        public override void Execute(MessageData data)
        {
            var game = data.GetTargetAs<Game>();
            var owner = game.GetEntity(data.sId) as Player;

            game.Spawn(new Shot(){Direction=owner.Direction,Position=owner.Position,Owner=owner});
        }

        protected override CommandSettings GetSettings()
        {
            return new CommandSettings();
        }
    }

    public class StartGameCommand : GameCommand
    {
        public override string Slug => "startGame";

        public override void Execute(MessageData data)
        {
            var game = data.GetTargetAs<Game>();
            game.SendCommand(ProxyMessageData.Create("gameSync",new GameSyncParams(game.Map,game.Players)));
            game.Start();

        }

        public class GameSyncParams
        {
            public Map map;

            public List<Player> players;

            public GameSyncParams(Map map, List<Player> players)
            {
                this.map = map;
                this.players = players;
            }
        }
    }
}