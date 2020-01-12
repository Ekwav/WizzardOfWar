using Coflnet;

namespace wow.Core.Extentions.WizzardOfWarGame
{
    public class JoinLobbyCommand : GameCommand
    {
        public override string Slug => "joinLobby";

        public override void Execute(MessageData data)
        {
            var name = data.GetAs<Params>().lobbyName;

            var game = GameManager.Games.Find(g=>g.Name == name);
            if(game == null)
            {
                throw new CoflnetException("game_not_found",$"The game with the name {name} was not found on this server ");
            }

            
            game.AddPlayer(data);
        }


        public class Params
        {
            public string lobbyName;
        }
    }


}