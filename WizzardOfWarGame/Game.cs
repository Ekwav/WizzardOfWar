using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coflnet;
using Newtonsoft.Json;

namespace wow.Core.Extentions.WizzardOfWarGame
{
    public class Map
    {
        public bool[][] InternalMap = JsonConvert.DeserializeObject<bool[][]>("[[true,true,false,true,true,true,true,true,true,true,true,true,true,true,true],[true,false,false,false,false,false,false,true,false,false,false,false,false,false,true],[true,false,true,true,true,true,false,true,false,true,true,true,true,false,true],[true,false,true,false,false,false,false,false,false,false,false,false,true,false,true],[true,false,false,false,true,false,true,true,true,false,true,false,false,false,true],[true,true,true,false,true,false,false,false,false,false,true,false,true,true,true],[true,false,false,false,true,false,true,true,true,false,true,false,false,false,true],[true,false,true,false,false,false,false,false,false,false,false,false,true,false,true],[true,false,true,true,true,true,false,true,false,true,true,true,true,false,true],[true,false,false,false,false,false,false,true,false,false,false,false,false,false,true],[true,true,true,true,true,true,true,true,true,true,true,true,true,true,true]]");

        public bool IsFree(Position position)
        {
            if(position.x < 0 || position.x >= InternalMap.Length
                || position.y < 0 || position.y >= InternalMap[0].Length)
                {
                    return false;
                }
            return !InternalMap[position.x][position.y];
        }

        public Position GetCenter()
        {
            return new Position(){x=InternalMap.Length/2,y=InternalMap[0].Length/2};
        }
    }

    public class Game : Referenceable, ITicks
    {
        private static CommandController _commands = new CommandController(globalCommands);

        public Map Map = new Map(){};

        public Dictionary<SourceReference,Entity> Entities = new Dictionary<SourceReference, Entity>();

        public List<Player> Players = new List<Player>();

        public bool IsRunning;

        public string Name;

        public Random random = new Random(15);


        static Game()
        {
            _commands.RegisterCommand<MoveCommand>();
            _commands.RegisterCommand<ShootCommand>();
            _commands.RegisterCommand<StartGameCommand>();
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

        public void Start()
        {
            foreach (var item in Players)
            {
                // assingn spawn Pos
                item.Position = GetRespawnPos();

                Spawn(item);
            }

            // spawn monsters
            Spawn(new Enemy(){Position=Map.GetCenter(),Direction=Direction.Down});

            IsRunning = true;
        }

        public void AddPlayer(MessageData data)
        {
            var connection = (data as ProxyServerMessageData).Connection as WoWProxy;

            Task.Run(()=>{
                Thread.Sleep(10);
                connection.currentTarget = this.Id;
                Console.WriteLine($"The id is: {this.Id}");
            });


            var player = new Player(){Id=data.sId,Connection=connection};
            Players.Add(player);
            Entities.Add(player.Id,player);
        }

        public Direction GetFreeDirection(Position position, Direction commingFrom)
        {
            var available = new List<Direction>();
            var values = Enum.GetValues(typeof(Direction)).Cast<Direction>();
            foreach (var item in values)
            {
                if(item != Direction.None &&  Map.IsFree(DetermineNewPosition(position,item)))
                {
                    available.Add(item);
                }
            }

            if(available.Count > 1)
            {
                // remove the one we are coming from
                available.Remove(commingFrom);
            }
            if(available.Count == 0)
            {
                return Direction.None;
            }

            return available[random.Next(0,available.Count)];
        }

        public Position GetRespawnPos()
        {
            return new Position(){x=1,y=1};
        }

        public void SendCommand(ProxyMessageData commandData)
        {
            foreach (var player in Players)
            {
                player.SendCommand(commandData);
            }
        }

        public void Spawn(Entity entity)
        {
            entity.Game = this;
            entity.Id = SourceReference.NextLocalId;
            
            SendCommand(ProxyMessageData.Create("spawn",entity));
            Entities.Add(entity.Id,entity);
        }

    

        public void Tick()
        {
            // update 
            foreach (var item in Entities.Values)
            {
                if(!item.IsDead)
                    item.Tick();
            }

        }

        public void MoveEnity(SourceReference Id,Direction direction)
        {
            var entity = GetEntity(Id);
            entity.Direction = direction;

            var newPosition = DetermineNewPosition(entity.Position,direction);

            if(!Map.IsFree(newPosition)){
                entity.HitWall(direction);
                return;
            }


            // is there another entity?
            if(Entities.Values.Where(e=>e.Position == newPosition).Any())
            {
                // we have a collision ladies and gentleman
                var opponent = Entities.Values.Where(e=>e.Position == newPosition).First();

                // both die now
                opponent.Collision(entity);
                entity.Collision(opponent);
            }

            entity.Position = newPosition;
            SendCommand(ProxyMessageData.Create("moveTo",new MoveToParams(entity.Id,newPosition,entity.Direction)));
        }

        public class MoveToParams : IdContainer
        {
            public Position position;
            public Direction direction;

            public MoveToParams(SourceReference id, Position newPosition, Direction direction) : base(id)
            {
                this.position = newPosition;
                this.direction = direction;
            }
        }

        public static Position DetermineNewPosition(Position position,Direction direction)
        {
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


        public Entity GetEntity(SourceReference id)
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