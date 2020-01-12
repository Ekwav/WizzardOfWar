using System.Threading;
using System.Threading.Tasks;
using Coflnet;
using MessagePack;
using static JsonSerializer;

namespace wow.Core.Extentions.WizzardOfWarGame
{

    public class CreateLobbyCommand : CreationCommand
    {
        public override string Slug => "createLobby";

        public override Referenceable CreateResource(MessageData data)
        {
            var name = data.GetAs<Pramas>().Name;
            if(GameManager.Games.Exists(g=>g.Name == name))
            {
                throw new CoflnetException("game_already_exists",$"A game with the name {name} already exists");
            }

            var game = new Game(){Name=name};

            game.AddPlayer(data);

            GameManager.Games.Add(game);
            
            return game;
        }

        protected override CommandSettings GetSettings()
        {
            return new CommandSettings();
        }


        [MessagePackObject]
        public class Test
        {
            [Key("abc")]
            public int abc;
            [Key("i")]
            public string inner;
        }

        [MessagePackObject]
        public class Pramas : CreationCommand.CreationParamsBase
        {
            [Key(1)]
            public string Name;
        } 
    }
}