using P01_StudentSystem.Data;
using System;

namespace P01_StudentSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StudentSystemContext ctx = new StudentSystemContext())
            {
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();
            }
        }
    }
}
