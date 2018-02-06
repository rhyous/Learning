namespace Rhyous.CS6210.Hw1.Repository
{
    using Rhyous.CS6210.Hw1.Interfaces;
    using System.Data.Entity;

    internal partial class GenericDbContext<T> : DbContext
        where T : class, IEntity
    {
        public GenericDbContext()
            : base("name=DbContext")
        {
        }

        public DbSet<T> Entities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}