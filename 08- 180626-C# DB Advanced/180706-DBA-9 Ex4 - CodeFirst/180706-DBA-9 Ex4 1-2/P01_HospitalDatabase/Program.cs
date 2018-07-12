using P01_HospitalDatabase.Data;
using P01_HospitalDatabase.Initializer;

namespace P01_HospitalDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            using (HospitalContext ctx = new HospitalContext())
            {
                DatabaseInitializer.InitialSeed(ctx);
            }
        }
    } 
}
