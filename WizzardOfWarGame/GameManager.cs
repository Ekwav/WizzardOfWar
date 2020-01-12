using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace wow.Core.Extentions.WizzardOfWarGame
{
    public class GameManager
    {

        static GameManager()
        {
            // Tick all games
            SetIntervalThread(()=>{
                Parallel.ForEach(Games,(game,state,i)=>{
                    if(game.IsRunning)
                        game.Tick();
                });
            },50);
        }

        public static System.Threading.Timer SetIntervalThread(Action Act, int Interval)
        {
            var state = new InvokationCounter { Counter = 0 };
            Timer tmr = new Timer(new TimerCallback(_ => Act()), state, Interval, Interval);
            state.TimerObject = tmr;
            return tmr;
        }

        public static List<Game> Games = new List<Game>();

        class InvokationCounter
        {
            public long Counter;
            public Timer TimerObject;
        }
    }
}