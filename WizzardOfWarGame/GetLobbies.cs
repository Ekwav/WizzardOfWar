using Coflnet;

namespace wow.Core.Extentions.WizzardOfWarGame
{
    public class GetLobbies : Command
    {
        public override string Slug => "getLobbies";

        public override void Execute(MessageData data)
        {
            data.SendBack(data.SerializeAndSet(GameManager.Games));
        }

        protected override CommandSettings GetSettings()
        {
            return new CommandSettings();
        }
    }
}