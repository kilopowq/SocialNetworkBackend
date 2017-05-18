using System.Data.Entity;

namespace EFModels
{
    public partial class SNSContext : DbContext
    {
        public SNSContext()
            : base("NewSNS")
        {
        }

        public virtual DbSet<Conference> Conferences { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Friend> Friends { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Message>()
                .HasMany(e => e.Comments)
                .WithMany()
                .Map(e =>
                {
                    e.MapLeftKey("CommentTold");
                    e.MapRightKey("MessageId");
                    e.ToTable("tblComment");
                });

            modelBuilder.Entity<User>()
                .HasMany(e => e.Conferences)
                .WithMany(e => e.Members)
                .Map(e =>
                {
                    e.MapLeftKey("UserId");
                    e.MapRightKey("ConferenceId");
                    e.ToTable("tblConferenceMembers");
                });

            modelBuilder.Entity<Friend>()
                        .HasRequired(m => m.User1)
                        .WithMany(t => t.Friends1)
                        .HasForeignKey(m => m.User1_ID)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Friend>()
                        .HasRequired(m => m.User2)
                        .WithMany(t => t.Friends2)
                        .HasForeignKey(m => m.User2_ID)
                        .WillCascadeOnDelete(false);

        }
    }
}
