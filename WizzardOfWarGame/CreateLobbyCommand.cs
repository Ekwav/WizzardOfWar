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
            return new Game(){Name=data.GetAs<Pramas>().Name};
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