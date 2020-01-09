using System;
using Microsoft.EntityFrameworkCore;

namespace Exam.Models
{
    public class HtmlContext : DbContext
    {
        public DbSet<HtmlSource> Htmls { get; set; }
        public HtmlContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=exam4;Username=postgres;Password=зщыепкуы");
        }
    }
}
