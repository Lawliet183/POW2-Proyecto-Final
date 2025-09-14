using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Proyecto_Final.Server;

public partial class ProyectoEncuestaContext : DbContext
{
    public ProyectoEncuestaContext()
    {
    }

    public ProyectoEncuestaContext(DbContextOptions<ProyectoEncuestaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Auth> Auths { get; set; }

    public virtual DbSet<Choice> Choices { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Respondent> Respondents { get; set; }

    public virtual DbSet<Submission> Submissions { get; set; }

    public virtual DbSet<Survey> Surveys { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("User Id=root;password=query_sql;Host=localhost;Database=proyecto_encuesta;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.ToTable("answers", "proyecto_encuesta");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AnswerDate).HasColumnName("Answer_date");
            entity.Property(e => e.AnswerNumber).HasColumnName("Answer_number");
            entity.Property(e => e.AnswerText)
                .HasMaxLength(100)
                .HasColumnType("varchar")
                .HasColumnName("Answer_text");
            entity.Property(e => e.QuestionId).HasColumnName("Question_ID");
            entity.Property(e => e.SelectedChoiceId).HasColumnName("Selected_Choice_ID");
            entity.Property(e => e.SubmissionId).HasColumnName("Submission_ID");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("Answers_FK2");

            entity.HasOne(d => d.SelectedChoice).WithMany(p => p.Answers)
                .HasForeignKey(d => d.SelectedChoiceId)
                .HasConstraintName("Answers_FK3");

            entity.HasOne(d => d.Submission).WithMany(p => p.Answers)
                .HasForeignKey(d => d.SubmissionId)
                .HasConstraintName("Answers_FK1");

            entity.HasMany(d => d.Choices).WithMany(p => p.AnswersNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "AnswersChoice",
                    r => r.HasOne<Choice>().WithMany()
                        .HasForeignKey("ChoiceId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Answers_Choices_FK2"),
                    l => l.HasOne<Answer>().WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Answers_Choices_FK1"),
                    j =>
                    {
                        j.HasKey("AnswerId", "ChoiceId");
                        j.ToTable("answers_choices", "proyecto_encuesta");
                        j.IndexerProperty<int>("AnswerId").HasColumnName("Answer_ID");
                        j.IndexerProperty<int>("ChoiceId").HasColumnName("Choice_ID");
                    });
        });

        modelBuilder.Entity<Auth>(entity =>
        {
            entity.HasKey(e => e.RespondentId);

            entity.ToTable("auth", "proyecto_encuesta");

            entity.Property(e => e.RespondentId).HasColumnName("Respondent_ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("Created_at");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnType("varchar")
                .HasColumnName("Password_Hash");
            entity.Property(e => e.Role)
                .HasDefaultValueSql("respondent")
                .HasColumnType("enum");

            entity.HasOne(d => d.Respondent).WithOne(p => p.Auth)
                .HasForeignKey<Auth>(d => d.RespondentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Auth_FK1");
        });

        modelBuilder.Entity<Choice>(entity =>
        {
            entity.ToTable("choices", "proyecto_encuesta");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Label)
                .HasMaxLength(100)
                .HasColumnType("varchar");
            entity.Property(e => e.QuestionId).HasColumnName("Question_ID");
            entity.Property(e => e.Value)
                .HasMaxLength(100)
                .HasColumnType("varchar");

            entity.HasOne(d => d.Question).WithMany(p => p.Choices)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("Choices_FK1");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.ToTable("questions", "proyecto_encuesta");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IsRequired).HasColumnName("Is_required");
            entity.Property(e => e.MaxValue).HasColumnName("Max_value");
            entity.Property(e => e.MinValue).HasColumnName("Min_value");
            entity.Property(e => e.SurveyId).HasColumnName("Survey_ID");
            entity.Property(e => e.Text)
                .HasMaxLength(100)
                .HasColumnType("varchar");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnType("varchar");

            entity.HasOne(d => d.Survey).WithMany(p => p.Questions)
                .HasForeignKey(d => d.SurveyId)
                .HasConstraintName("Questions_FK1");
        });

        modelBuilder.Entity<Respondent>(entity =>
        {
            entity.ToTable("respondents", "proyecto_encuesta");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("Created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(254)
                .HasColumnType("varchar");
            entity.Property(e => e.ExternalId)
                .HasMaxLength(50)
                .HasColumnType("varchar")
                .HasColumnName("External_ID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnType("varchar");
        });

        modelBuilder.Entity<Submission>(entity =>
        {
            entity.ToTable("submissions", "proyecto_encuesta");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.RespondentId).HasColumnName("Respondent_ID");
            entity.Property(e => e.StartedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("Started_at");
            entity.Property(e => e.SubmittedAt).HasColumnName("Submitted_at");
            entity.Property(e => e.SurveyId).HasColumnName("Survey_ID");

            entity.HasOne(d => d.Respondent).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.RespondentId)
                .HasConstraintName("Submissions_FK2");

            entity.HasOne(d => d.Survey).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.SurveyId)
                .HasConstraintName("Submissions_FK1");
        });

        modelBuilder.Entity<Survey>(entity =>
        {
            entity.ToTable("surveys", "proyecto_encuesta");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClosesAt).HasColumnName("Closes_at");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("Created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnType("varchar");
            entity.Property(e => e.IsAnonymous).HasColumnName("Is_anonymous");
            entity.Property(e => e.OpensAt).HasColumnName("Opens_at");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnType("varchar");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnType("varchar");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
