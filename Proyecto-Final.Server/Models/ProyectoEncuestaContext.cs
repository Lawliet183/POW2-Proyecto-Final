using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

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


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("answers");

            entity.HasIndex(e => e.SubmissionId, "Answers_FK1");

            entity.HasIndex(e => e.QuestionId, "Answers_FK2");

            entity.HasIndex(e => e.SelectedChoiceId, "Answers_FK3");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AnswerDate)
                .HasColumnType("datetime")
                .HasColumnName("Answer_date");
            entity.Property(e => e.AnswerNumber).HasColumnName("Answer_number");
            entity.Property(e => e.AnswerText)
                .HasMaxLength(100)
                .HasColumnName("Answer_text");
            entity.Property(e => e.QuestionId).HasColumnName("Question_ID");
            entity.Property(e => e.SelectedChoiceId).HasColumnName("Selected_Choice_ID");
            entity.Property(e => e.SubmissionId).HasColumnName("Submission_ID");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Answers_FK2");

            entity.HasOne(d => d.SelectedChoice).WithMany(p => p.Answers)
                .HasForeignKey(d => d.SelectedChoiceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Answers_FK3");

            entity.HasOne(d => d.Submission).WithMany(p => p.Answers)
                .HasForeignKey(d => d.SubmissionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Answers_FK1");

            entity.HasMany(d => d.Choices).WithMany(p => p.AnswersNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "AnswersChoice",
                    r => r.HasOne<Choice>().WithMany()
                        .HasForeignKey("ChoiceId")
                        .HasConstraintName("Answers_Choices_FK2"),
                    l => l.HasOne<Answer>().WithMany()
                        .HasForeignKey("AnswerId")
                        .HasConstraintName("Answers_Choices_FK1"),
                    j =>
                    {
                        j.HasKey("AnswerId", "ChoiceId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("answers_choices");
                        j.HasIndex(new[] { "ChoiceId" }, "Answers_Choices_FK2");
                        j.IndexerProperty<int>("AnswerId").HasColumnName("Answer_ID");
                        j.IndexerProperty<int>("ChoiceId").HasColumnName("Choice_ID");
                    });
        });

        modelBuilder.Entity<Auth>(entity =>
        {
            entity.HasKey(e => e.RespondentId).HasName("PRIMARY");

            entity.ToTable("auth");

            entity.Property(e => e.RespondentId)
                .ValueGeneratedNever()
                .HasColumnName("Respondent_ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("Password_Hash");
            entity.Property(e => e.Role)
                .HasDefaultValueSql("'respondent'")
                .HasColumnType("enum('admin','respondent')");

            entity.HasOne(d => d.Respondent).WithOne(p => p.Auth)
                .HasForeignKey<Auth>(d => d.RespondentId)
                .HasConstraintName("Auth_FK1");
        });

        modelBuilder.Entity<Choice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("choices");

            entity.HasIndex(e => e.QuestionId, "Choices_FK1");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Label).HasMaxLength(100);
            entity.Property(e => e.QuestionId).HasColumnName("Question_ID");
            entity.Property(e => e.Value).HasMaxLength(100);

            entity.HasOne(d => d.Question).WithMany(p => p.Choices)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Choices_FK1");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("questions");

            entity.HasIndex(e => e.SurveyId, "Questions_FK1");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IsRequired).HasColumnName("Is_required");
            entity.Property(e => e.MaxValue).HasColumnName("Max_value");
            entity.Property(e => e.MinValue).HasColumnName("Min_value");
            entity.Property(e => e.SurveyId).HasColumnName("Survey_ID");
            entity.Property(e => e.Text).HasMaxLength(200);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.Survey).WithMany(p => p.Questions)
                .HasForeignKey(d => d.SurveyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Questions_FK1");
        });

        modelBuilder.Entity<Respondent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("respondents");

            entity.HasIndex(e => e.Email, "uq_respondents_email").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.Email).HasMaxLength(254);
            entity.Property(e => e.ExternalId)
                .HasMaxLength(50)
                .HasColumnName("External_ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Submission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("submissions");

            entity.HasIndex(e => e.SurveyId, "Submissions_FK1");

            entity.HasIndex(e => e.RespondentId, "Submissions_FK2");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.RespondentId).HasColumnName("Respondent_ID");
            entity.Property(e => e.StartedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("Started_at");
            entity.Property(e => e.SubmittedAt)
                .HasColumnType("datetime")
                .HasColumnName("Submitted_at");
            entity.Property(e => e.SurveyId).HasColumnName("Survey_ID");

            entity.HasOne(d => d.Respondent).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.RespondentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Submissions_FK2");

            entity.HasOne(d => d.Survey).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.SurveyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Submissions_FK1");
        });

        modelBuilder.Entity<Survey>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("surveys");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClosesAt)
                .HasColumnType("datetime")
                .HasColumnName("Closes_at");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.IsAnonymous).HasColumnName("Is_anonymous");
            entity.Property(e => e.OpensAt)
                .HasColumnType("datetime")
                .HasColumnName("Opens_at");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
