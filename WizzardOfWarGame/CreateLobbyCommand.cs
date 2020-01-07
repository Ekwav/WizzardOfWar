using Coflnet;

namespace wow.Core.Extentions.WizzardOfWarGame
{
    public class CreateLobbyCommand : Command
    {
        public override string Slug => "createLobby";

        public override void Execute(MessageData data)
        {
            data.SendBack(new MessageData(default(SourceReference),0,"hi","Test Response"));
        }

        protected override CommandSettings GetSettings()
        {
            return new CommandSettings();
        }
    }
}