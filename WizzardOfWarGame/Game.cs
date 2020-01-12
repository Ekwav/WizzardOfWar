using System.Collections.Generic;
using System.Linq;
using Coflnet;

namespace wow.Core.Extentions.WizzardOfWarGame
{
    public class Game : Referenceable, ITicks
    {
        private static CommandController _commands = new CommandController(globalCommands);

        public bool[][] Map;

        public Dictionary<SourceReference,Entity> Entities;

        public List<Player> Players;

        public bool IsRunning;

        public string Name;


        static Game()
        {
            _commands.RegisterCommand<MoveCommand>();
        }

        public override CommandController GetCommandController()
        {
            return _commands;
        }

        public void Kill(SourceReference id)
        {
            SendCommand(ProxyMessageData.Create("kill",new IdContainer(id)));
            Entities[id].IsDead = true;
        }

        public Position GetRespawnPos()
        {
            return new Position(){x=0,y=0};
        }

        public void SendCommand(ProxyMessageData commandData)
        {
            foreach (var player in Players)
            {
                player.SendCommand(commandData);
            }
        }

        public void Tick()
        {
            // update 
            foreach (var item in Entities.Values)
            {
                item.Tick();
            }

        }

        public void MoveEnity(SourceReference Id,Direction direction)
        {
            var entity = GetEntity(Id);

            var newPosition = DetermineNewPosition(entity,direction);

            if(Map[newPosition.x][newPosition.y]){
                entity.HitWall(direction);
                return;
            }


            // is there another entity?
            if(Entities.Values.Where(e=>e.Position == newPosition).Any())
            {
                // we have a collision ladies and gentleman
                var opponent = Entities.Values.Where(e=>e.Position == newPosition).First();

                // both die now
                opponent.Die(entity);
                entity.Die(opponent);
            }
        }

        Position DetermineNewPosition(Entity entity,Direction direction)
        {
            var position = entity.Position;
            switch(direction)
            {
                case Direction.Up:
                    position.y--;
                    break;
                case Direction.Down:
                    position.y++;
                    break;
                case Direction.Left:
                    position.x--;
                    break;
                case Direction.Right:
                    position.x++;
                    break;
            }
            return position;
        }


        Entity GetEntity(SourceReference id)
        {
            return Entities[id];
        }
    }

    public class IdContainer
    {
        public SourceReference Id;

        public IdContainer(SourceReference id)
        {
            Id = id;
        }
    }
}