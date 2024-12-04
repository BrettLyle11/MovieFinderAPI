using Microsoft.EntityFrameworkCore;
using MovieFinderAPI.Models;

namespace MovieFinderAPI.Data
{
    public class MovieFinderContext : DbContext
    {
        public MovieFinderContext(DbContextOptions<MovieFinderContext> options)
        : base(options)
        {
        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<GenreToMovie> GenreToMovies { get; set; }
        public DbSet<StreamingService> StreamingServices { get; set; }
        public DbSet<MovieStreamedOn> MovieStreamedOns { get; set; }
        public DbSet<RatingCompany> RatingCompanies { get; set; }
        public DbSet<MovieRating> MovieRatings { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<DirectedBy> DirectedBys { get; set; }
        public DbSet<ProductionCompany> ProductionCompanies { get; set; }
        public DbSet<ProducedBy> ProducedBys { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<ActedIn> ActedIns { get; set; }
        public DbSet<MovieFinderUser> MovieFinderUsers { get; set; }
        public DbSet<WatchHistory> WatchHistories { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistMovies> PlaylistMovies { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<ManagesUser> ManagesUsers { get; set; }
        public DbSet<ManagesMovie> ManagesMovies { get; set; }
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieSearchDetails>().HasNoKey();
            // Movie
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("Movie"); // Map to 'Movie' table
                entity.HasKey(e => new { e.Year, e.Name });
                entity.Property(e => e.Year).HasColumnName("year");
                entity.Property(e => e.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasColumnType("VARCHAR(MAX)");
                entity.Property(e => e.Image).HasColumnType("VARCHAR(MAX)");
            });

            modelBuilder.Entity<GenreToMovie>(entity =>
            {
                entity.ToTable("GenreToMovie");
                entity.HasKey(e => new { e.Year, e.Name, e.Genre });

                entity.Property(e => e.Year).HasColumnName("year");
                entity.Property(e => e.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Genre).HasColumnName("Genre").HasMaxLength(20).IsRequired();

                // Define the foreign key relationship
                entity.HasOne(e => e.Movie)
                    .WithMany(m => m.Genres)
                    .HasForeignKey(e => new { e.Year, e.Name });

            });

            // StreamingService
            modelBuilder.Entity<StreamingService>(entity =>
            {
                entity.ToTable("StreamingService");
                entity.HasKey(e => e.StreamingServiceName);
                entity.Property(e => e.StreamingServiceName).HasMaxLength(50).IsRequired();
            });

            // MovieStreamedOn
            modelBuilder.Entity<MovieStreamedOn>(entity =>
            {
                entity.ToTable("MovieStreamedOn");
                entity.HasKey(e => new { e.Year, e.Name, e.StreamingServiceName });

                entity.Property(e => e.Year).HasColumnName("year");
                entity.Property(e => e.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();

                entity.HasOne(e => e.Movie)
                    .WithMany(m => m.MovieStreamedOns)
                    .HasForeignKey(e => new { e.Year, e.Name });

                entity.HasOne(e => e.StreamingService)
                    .WithMany(s => s.MovieStreamedOns)
                    .HasForeignKey(e => e.StreamingServiceName);
            });

            // RatingCompany
            modelBuilder.Entity<RatingCompany>(entity =>
            {
                entity.ToTable("RatingCompany");
                entity.HasKey(e => e.RatingCompanyName);
                entity.Property(e => e.RatingCompanyName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.RatingScale).HasMaxLength(20).IsRequired();
            });

            // MovieRating
            modelBuilder.Entity<MovieRating>(entity =>
            {
                entity.ToTable("MovieRating");
                entity.HasKey(e => new { e.Year, e.Name, e.RatingCompanyName });

                entity.HasOne(e => e.Movie)
                    .WithMany(m => m.MovieRatings)
                    .HasForeignKey(e => new { e.Year, e.Name });

                entity.HasOne(e => e.RatingCompany)
                    .WithMany(r => r.MovieRatings)
                    .HasForeignKey(e => e.RatingCompanyName);

                entity.Property(e => e.Score).HasMaxLength(10).IsRequired();
                entity.Property(e => e.Summary).HasMaxLength(100);
            });

            // Director
            modelBuilder.Entity<Director>(entity =>
            {
                entity.ToTable("Director");
                entity.HasKey(e => e.DirectorName);
                entity.Property(e => e.DirectorName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Gender).HasMaxLength(10).HasColumnName("Gender");
                entity.Property(e => e.Age)
                        .HasColumnName("Age");
            });

            // DirectedBy
            modelBuilder.Entity<DirectedBy>(entity =>
            {
                entity.ToTable("Directed_By");
                entity.HasKey(e => new { e.Year, e.Name, e.DirectorName });

                entity.Property(e => e.Year).HasColumnName("year");
                entity.Property(e => e.Name).HasColumnName("Name").HasMaxLength(100).IsRequired();

                entity.HasOne(e => e.Movie)
                    .WithMany(m => m.DirectedBys)
                    .HasForeignKey(e => new { e.Year, e.Name });

                entity.HasOne(e => e.Director)
                    .WithMany(d => d.DirectedBys)
                    .HasForeignKey(e => e.DirectorName);
            });

            // ProductionCompany
            modelBuilder.Entity<ProductionCompany>(entity =>
            {
                entity.ToTable("ProductionCompany");
                entity.HasKey(e => e.ProductionCompanyName);
                entity.Property(e => e.ProductionCompanyName).HasMaxLength(100).IsRequired();
            });

            // ProducedBy
            modelBuilder.Entity<ProducedBy>(entity =>
            {
                entity.ToTable("Produced_By");
                entity.HasKey(e => new { e.Year, e.Name, e.ProductionCompanyName });

                entity.HasOne(e => e.Movie)
                    .WithMany(m => m.ProducedBys)
                    .HasForeignKey(e => new { e.Year, e.Name });

                entity.HasOne(e => e.ProductionCompany)
                    .WithMany(p => p.ProducedBys)
                    .HasForeignKey(e => e.ProductionCompanyName);
            });

            // Actor
            modelBuilder.Entity<Actor>(entity =>
            {
                entity.ToTable("Actor");
                entity.HasKey(e => e.ActorName);
                entity.Property(e => e.ActorName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Gender).HasMaxLength(10).HasColumnName("Gender");
                entity.Property(e => e.Age)
                        .HasColumnName("Age");
            });

            // ActedIn
            modelBuilder.Entity<ActedIn>(entity =>
            {
                entity.ToTable("Acted_In");
                entity.HasKey(e => new { e.Year, e.Name, e.ActorName });

                entity.HasOne(e => e.Movie)
                    .WithMany(m => m.ActedIns)
                    .HasForeignKey(e => new { e.Year, e.Name });

                entity.HasOne(e => e.Actor)
                    .WithMany(a => a.ActedIns)
                    .HasForeignKey(e => e.ActorName);
            });

            // MovieFinderUser
            modelBuilder.Entity<MovieFinderUser>(entity =>
            {
                entity.HasKey(e => e.UserID);

                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Username).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Password).HasMaxLength(50).IsRequired();
                entity.Property(e => e.FavouriteGenre).HasMaxLength(50);
            });

            // WatchHistory
            modelBuilder.Entity<WatchHistory>(entity =>
            {
                entity.HasKey(e => new { e.UserID, e.Year, e.Name });

                entity.HasOne(e => e.MovieFinderUser)
                    .WithMany(u => u.WatchHistories)
                    .HasForeignKey(e => e.UserID);

                entity.HasOne(e => e.Movie)
                    .WithMany(m => m.WatchHistories)
                    .HasForeignKey(e => new { e.Year, e.Name });
            });

            // Playlist
            modelBuilder.Entity<Playlist>(entity =>
            {
                entity.HasKey(e => new { e.UserID, e.PlaylistName });

                entity.HasOne(e => e.MovieFinderUser)
                    .WithMany(u => u.Playlists)
                    .HasForeignKey(e => e.UserID);

                entity.Property(e => e.PlaylistName).HasMaxLength(100).IsRequired();
            });

            // PlaylistMovies
            modelBuilder.Entity<PlaylistMovies>(entity =>
            {
                entity.HasKey(e => new { e.UserID, e.PlaylistName });


                entity.Property(e => e.PlaylistName).HasMaxLength(100).IsRequired();
            });

            // Admin
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.AdminID);

                entity.Property(e => e.Password).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Username).HasMaxLength(50).IsRequired();
                entity.HasIndex(e => e.Username).IsUnique();
            });

            // ManagesUser
            modelBuilder.Entity<ManagesUser>(entity =>
            {
                entity.ToTable("Manages_User");
                entity.HasKey(e => new { e.AdminID, e.UserID });

                entity.HasOne(e => e.Admin)
                    .WithMany(a => a.ManagesUsers)
                    .HasForeignKey(e => e.AdminID);

                entity.HasOne(e => e.MovieFinderUser)
                    .WithMany(u => u.ManagedByAdmins)
                    .HasForeignKey(e => e.UserID);

                entity.Property(e => e.Date).IsRequired();
            });

            // ManagesMovie
            modelBuilder.Entity<ManagesMovie>(entity =>
            {
                entity.ToTable("Manages_Movie");
                entity.HasKey(e => new { e.AdminID, e.Year, e.Name });

                entity.HasOne(e => e.Admin)
                    .WithMany(a => a.ManagesMovies)
                    .HasForeignKey(e => e.AdminID);

                entity.HasOne(e => e.Movie)
                    .WithMany(m => m.ManagedByAdmins)
                    .HasForeignKey(e => new { e.Year, e.Name });

                entity.Property(e => e.Date).IsRequired();
            });
        }
    }
}