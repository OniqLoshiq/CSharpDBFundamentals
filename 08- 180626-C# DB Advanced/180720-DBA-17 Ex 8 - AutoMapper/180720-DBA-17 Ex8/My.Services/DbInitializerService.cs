using Microsoft.EntityFrameworkCore;
using My.Data;
using My.Services.Interfaces;

namespace My.Services
{
    public class DbInitializerService : IDbInitializerService
    {
        private readonly MyDbContext db;

        public DbInitializerService(MyDbContext db)
        {
            this.db = db;
        }

        public void InitializeDatebase()
        {
            this.db.Database.Migrate();
        }
    }
}
