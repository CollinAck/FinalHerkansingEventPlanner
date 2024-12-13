using FinalHerkansingEventPlanner.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalHerkansingEventPlanner.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<LocationChoice> LocationChoices { get; set; }
        public DbSet<CategoryChoice> CategoryChoices { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(t => t.EventId);


            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Participant)
                .WithMany(p => p.Tickets)
                .HasForeignKey(t => t.ParticipantId);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.LocationChoice)
                .WithMany(l => l.Events)
                .HasForeignKey(e => e.LocationChoiceId);


            modelBuilder.Entity<Event>()
                .HasOne(e => e.CategoryChoice)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.CategoryChoiceId);
 


            base.OnModelCreating(modelBuilder);


            // Dummy data om tickets te testen, migratie -> AddDummy 
            // Zonder accounts staat in de index de waarde op 1 --> value="1"
            // Men staat standaard op onderstaande dummy, geen onderscheid gast/lid als die deelnemer zal zijn
            modelBuilder.Entity<Participant>().HasData(
                new Participant
                {
                    ParticipantId = 1,
                    Name = "Test",
                    Email = "test@test.com"
                }
             );
        }
    }
}