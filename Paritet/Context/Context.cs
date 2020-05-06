using Paritet.Model;
using System.Data.Entity;

namespace Paritet
{
    public class Context : DbContext 
    {
        public Context() : base("DbConnection") { }

        public virtual DbSet<Person> People { get; set; }
    }
}