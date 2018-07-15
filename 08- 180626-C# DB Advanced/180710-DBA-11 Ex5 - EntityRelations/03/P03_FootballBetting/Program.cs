using P03_FootballBetting.Data;

namespace P03_FootballBetting
{
    class Program
    {
        static void Main(string[] args)
        {
            using (FootballBettingContext ctx = new FootballBettingContext())
            {
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();
            }
        }
    }
}
