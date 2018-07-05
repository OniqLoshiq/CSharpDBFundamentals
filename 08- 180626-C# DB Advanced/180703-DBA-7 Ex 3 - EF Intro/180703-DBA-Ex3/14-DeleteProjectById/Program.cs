using P02_DatabaseFirst.Data;
using System;
using System.IO;
using System.Linq;

namespace _14_DeleteProjectById
{
    class Program
    {
        static void Main(string[] args)
        {
            int idToDelete = 2;

            using (var ctx = new SoftUniContext())
            {
                var projects = ctx.EmployeesProjects.Where(p => p.ProjectId == idToDelete);
                ctx.EmployeesProjects.RemoveRange(projects);

                var project = ctx.Projects.Find(idToDelete);
                ctx.Projects.Remove(project);

                ctx.SaveChanges();

                var projectsToPrint = ctx.Projects.Select(p => p.Name).Take(10);

                using (StreamWriter sw = new StreamWriter("../../../../14_DeleteProjectById.txt"))
                {
                    foreach (var p in projectsToPrint)
                    {
                        sw.WriteLine(p);
                    }
                }
            }
        }
    }
}
