using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ASE_Election_Portal_G20.Models;

public partial class ElectionPortalG20Context : DbContext
{
    public ElectionPortalG20Context()
    {
    }

    public ElectionPortalG20Context(DbContextOptions<ElectionPortalG20Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<CandidateDocument> CandidateDocuments { get; set; }

    public virtual DbSet<County> Counties { get; set; }

    public virtual DbSet<Election> Elections { get; set; }

    public virtual DbSet<ElectionType> ElectionTypes { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vote> Votes { get; set; }

    public virtual DbSet<Voter> Voters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__719FE4E80C449F4E");

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.AdminName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Admins)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Admins__UserID__693CA210");
        });

        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.HasKey(e => e.CandidateId).HasName("PK__Candidat__DF539BFCFCD91DB4");

            entity.Property(e => e.CandidateId).HasColumnName("CandidateID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.ElectionTypeId).HasColumnName("ElectionTypeID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NominatedPositionId).HasColumnName("NominatedPositionID");
            entity.Property(e => e.PoliticalParty)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.CountyNavigation).WithMany(p => p.Candidates)
                .HasForeignKey(d => d.County)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Candidate__Count__06CD04F7");

            entity.HasOne(d => d.ElectionType).WithMany(p => p.Candidates)
                .HasForeignKey(d => d.ElectionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Candidate__Elect__04E4BC85");

            entity.HasOne(d => d.NominatedPosition).WithMany(p => p.Candidates)
                .HasForeignKey(d => d.NominatedPositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Candidate__Nomin__03F0984C");

            entity.HasOne(d => d.StateNavigation).WithMany(p => p.Candidates)
                .HasForeignKey(d => d.State)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Candidate__State__05D8E0BE");

            entity.HasOne(d => d.User).WithMany(p => p.Candidates)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Candidate__UserI__02FC7413");
        });

        modelBuilder.Entity<CandidateDocument>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("PK__Candidat__1ABEEF6F5140EFFA");

            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.CandidateId).HasColumnName("CandidateID");
            entity.Property(e => e.DocumentName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DocumentUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DocumentURL");

            entity.HasOne(d => d.Candidate).WithMany(p => p.CandidateDocuments)
                .HasForeignKey(d => d.CandidateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Candidate__Candi__0E6E26BF");
        });

        modelBuilder.Entity<County>(entity =>
        {
            entity.HasKey(e => e.CountyId).HasName("PK__Counties__B68F9DF7A30C24AD");

            entity.Property(e => e.CountyId).HasColumnName("CountyID");
            entity.Property(e => e.CountyName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StateId).HasColumnName("StateID");

            entity.HasOne(d => d.State).WithMany(p => p.Counties)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Counties__StateI__6E01572D");
        });

        modelBuilder.Entity<Election>(entity =>
        {
            entity.HasKey(e => e.ElectionId).HasName("PK__Election__C626C7C6C079A5CF");

            entity.Property(e => e.ElectionId).HasColumnName("ElectionID");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ElectionTypeId).HasColumnName("ElectionTypeID");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.PositionId).HasColumnName("PositionID");
            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.HasOne(d => d.ElectionType).WithMany(p => p.Elections)
                .HasForeignKey(d => d.ElectionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Elections__IsDel__151B244E");

            entity.HasOne(d => d.Position).WithMany(p => p.Elections)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Elections__Posit__160F4887");
        });

        modelBuilder.Entity<ElectionType>(entity =>
        {
            entity.HasKey(e => e.ElectionTypeId).HasName("PK__Election__5044E6FFDEB928D2");

            entity.Property(e => e.ElectionTypeId).HasColumnName("ElectionTypeID");
            entity.Property(e => e.ElectionTypeName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("PK__Position__60BB9A59DD753F6C");

            entity.Property(e => e.PositionId).HasColumnName("PositionID");
            entity.Property(e => e.ElectionTypeId).HasColumnName("ElectionTypeID");
            entity.Property(e => e.PositionName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.ElectionType).WithMany(p => p.Positions)
                .HasForeignKey(d => d.ElectionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Positions__Elect__72C60C4A");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.StateId).HasName("PK__States__C3BA3B5A3644239B");

            entity.Property(e => e.StateId).HasColumnName("StateID");
            entity.Property(e => e.StateName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACE9E9010F");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Vote>(entity =>
        {
            entity.HasKey(e => e.VoteId).HasName("PK__Votes__52F015E2D39F3913");

            entity.Property(e => e.VoteId).HasColumnName("VoteID");
            entity.Property(e => e.CandidateId).HasColumnName("CandidateID");
            entity.Property(e => e.ElectionId).HasColumnName("ElectionID");
            entity.Property(e => e.VoterId).HasColumnName("VoterID");

            entity.HasOne(d => d.Candidate).WithMany(p => p.Votes)
                .HasForeignKey(d => d.CandidateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Votes__Candidate__19DFD96B");

            entity.HasOne(d => d.Election).WithMany(p => p.Votes)
                .HasForeignKey(d => d.ElectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Votes__ElectionI__1AD3FDA4");

            entity.HasOne(d => d.Voter).WithMany(p => p.Votes)
                .HasForeignKey(d => d.VoterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Votes__VoterID__18EBB532");
        });

        modelBuilder.Entity<Voter>(entity =>
        {
            entity.HasKey(e => e.VoterId).HasName("PK__Voters__12D25BD828D445AA");

            entity.Property(e => e.VoterId).HasColumnName("VoterID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.CountyNavigation).WithMany(p => p.Voters)
                .HasForeignKey(d => d.County)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Voters__County__0B91BA14");

            entity.HasOne(d => d.StateNavigation).WithMany(p => p.Voters)
                .HasForeignKey(d => d.State)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Voters__State__0A9D95DB");

            entity.HasOne(d => d.User).WithMany(p => p.Voters)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Voters__UserID__09A971A2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
