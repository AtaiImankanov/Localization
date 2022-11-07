using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace lavAspMvclast.Models
{
    public class MobileContext : IdentityDbContext<User,IdentityRole<int>,int>
    {
        public DbSet<ToDoTask> ToDoTasks { get; set; }

        public MobileContext(DbContextOptions<MobileContext> options) : base(options)
        {

        }
    }
}
