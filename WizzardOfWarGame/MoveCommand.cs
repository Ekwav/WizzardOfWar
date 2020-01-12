using Coflnet;

namespace wow.Core.Extentions.WizzardOfWarGame
{
    public class MoveCommand : Command
    {
        public override string Slug => "move";

        public override void Execute(MessageData data)
        {
            var game = data.GetTargetAs<Game>();

            game.MoveEnity(data.sId,data.GetAs<Direction>());


        }

        protected override CommandSettings GetSettings()
        {
            return new CommandSettings();
        }
    }
}