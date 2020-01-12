using Coflnet;

namespace wow.Core.Extentions.WizzardOfWarGame
{
    public abstract class GameCommand : Command
    {
        protected override CommandSettings GetSettings()
        {
            return new CommandSettings();
        }
    }
}