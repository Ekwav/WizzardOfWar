using Coflnet;

namespace wow.Core.Extentions.WizzardOfWarGame
{
    public class GetLobbies : Command
    {
        public override string Slug => "getLobbies";

        public override void Execute(MessageData data)
        {
            data.SerializeAndSet(GameManager.Games);
            data.type = "lobbiesResponse";
            data.SendBack(data);
        }

        protected override CommandSettings GetSettings()
        {
            return new CommandSettings();
        }
    }
}