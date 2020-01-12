using Coflnet;

namespace wow.Core.Extentions.WizzardOfWarGame
{
    public class WOWExtention: IRegisterCommands
    {
        public void RegisterCommands(CommandController controller)
        {
            controller.RegisterCommand<CreateLobbyCommand>();
            controller.RegisterCommand<GetLobbies>();
            // part of the core
            //controller.RegisterCommand<RegisterDevice>();
        }
    }
}