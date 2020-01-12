namespace wow.Core.Extentions.WizzardOfWarGame
{
    public abstract class NonPlayerEnity : Entity
    {
        public long TicksBetweenUpdate = 10;

        public long TicksSinceUpdate = 0;

        public override void Tick()
        {
            lock(this)
            {
                TicksSinceUpdate++;

                if(TicksSinceUpdate >= TicksBetweenUpdate)
                {
                    Update();
                    TicksSinceUpdate = 0;
                }
            }
        }

        public abstract void Update();
    }
}