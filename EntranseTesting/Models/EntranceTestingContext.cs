using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EntranseTesting.Models;

public partial class EntranceTestingContext : DbContext
{
    public EntranceTestingContext()
    {
    }

    public EntranceTestingContext(DbContextOptions<EntranceTestingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ElementOfArrangement> ElementOfArrangements { get; set; }

    public virtual DbSet<ElementOfChoose> ElementOfChooses { get; set; }

    public virtual DbSet<ElementOfEquality> ElementOfEqualities { get; set; }

    public virtual DbSet<ElementOfGroup> ElementOfGroups { get; set; }

    public virtual DbSet<ElementOfPutting> ElementOfPuttings { get; set; }

    public virtual DbSet<ElementOrientation> ElementOrientations { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionImage> QuestionImages { get; set; }

    public virtual DbSet<QuestionVariant> QuestionVariants { get; set; }

    public virtual DbSet<RatioOfElementEquality> RatioOfElementEqualities { get; set; }

    public virtual DbSet<TextOfPutting> TextOfPuttings { get; set; }

    public virtual DbSet<UserResponse> UserResponses { get; set; }

    public virtual DbSet<UserResponseArrangement> UserResponseArrangements { get; set; }

    public virtual DbSet<UserResponseChooseAnswer> UserResponseChooseAnswers { get; set; }

    public virtual DbSet<UserResponseMatchTheElement> UserResponseMatchTheElements { get; set; }

    public virtual DbSet<UserResponseMatchTheValue> UserResponseMatchTheValues { get; set; }

    public virtual DbSet<UserResponseMultiplyAnswer> UserResponseMultiplyAnswers { get; set; }

    public virtual DbSet<UserSession> UserSessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=ngknn.ru;Port=5442;Database=entrance_testing;Username=43P;Password=444444");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("category_pk");

            entity.ToTable("category");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdOrientation).HasColumnName("id_orientation");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");

            entity.HasOne(d => d.IdOrientationNavigation).WithMany(p => p.Categories)
                .HasForeignKey(d => d.IdOrientation)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("category_element_orientation_fk");
        });

        modelBuilder.Entity<ElementOfArrangement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("element_of_arrangement_pk");

            entity.ToTable("element_of_arrangement");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdQuestion).HasColumnName("id_question");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Position).HasColumnName("position");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.ElementOfArrangements)
                .HasForeignKey(d => d.IdQuestion)
                .HasConstraintName("element_of_arrangement_question_fk");
        });

        modelBuilder.Entity<ElementOfChoose>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("element_of_choose_pk");

            entity.ToTable("element_of_choose");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Correctly).HasColumnName("correctly");
            entity.Property(e => e.IdQuestion).HasColumnName("id_question");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.ElementOfChooses)
                .HasForeignKey(d => d.IdQuestion)
                .HasConstraintName("element_of_choose_question_fk");
        });

        modelBuilder.Entity<ElementOfEquality>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("element_of_equality_pk");

            entity.ToTable("element_of_equality");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdQuestion).HasColumnName("id_question");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.ElementOfEqualities)
                .HasForeignKey(d => d.IdQuestion)
                .HasConstraintName("element_of_equality_question_fk");
        });

        modelBuilder.Entity<ElementOfGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("element_of_group_pk");

            entity.ToTable("element_of_group");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdGroup).HasColumnName("id_group");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.RatioNumeri).HasColumnName("ratio_numeri");

            entity.HasOne(d => d.IdGroupNavigation).WithMany(p => p.ElementOfGroups)
                .HasForeignKey(d => d.IdGroup)
                .HasConstraintName("element_of_group_group_fk");
        });

        modelBuilder.Entity<ElementOfPutting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("element_of_putting_pk");

            entity.ToTable("element_of_putting");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Correctly).HasColumnName("correctly");
            entity.Property(e => e.IdText).HasColumnName("id_text");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");

            entity.HasOne(d => d.IdTextNavigation).WithMany(p => p.ElementOfPuttings)
                .HasForeignKey(d => d.IdText)
                .HasConstraintName("element_of_putting_text_of_putting_fk");
        });

        modelBuilder.Entity<ElementOrientation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("element_orientation_pk");

            entity.ToTable("element_orientation");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("group_pk");

            entity.ToTable("group");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdQuestion).HasColumnName("id_question");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.Groups)
                .HasForeignKey(d => d.IdQuestion)
                .HasConstraintName("group_question_fk");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("question_pk");

            entity.ToTable("question");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Questions)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("question_category_fk");
        });

        modelBuilder.Entity<QuestionImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("question_image_pk");

            entity.ToTable("question_image");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdQuestion).HasColumnName("id_question");
            entity.Property(e => e.Image).HasColumnName("image");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.QuestionImages)
                .HasForeignKey(d => d.IdQuestion)
                .HasConstraintName("question_image_question_fk");
        });

        modelBuilder.Entity<QuestionVariant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("question_variant_pk");

            entity.ToTable("question_variant");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdQuestion).HasColumnName("id_question");
            entity.Property(e => e.Variant).HasColumnName("variant");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.QuestionVariants)
                .HasForeignKey(d => d.IdQuestion)
                .HasConstraintName("question_variant_question_fk");
        });

        modelBuilder.Entity<RatioOfElementEquality>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ratio_of_element_equality_pk");

            entity.ToTable("ratio_of_element_equality");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdElement1).HasColumnName("id_element1");
            entity.Property(e => e.IdElement2).HasColumnName("id_element2");

            entity.HasOne(d => d.IdElement1Navigation).WithMany(p => p.RatioOfElementEqualityIdElement1Navigations)
                .HasForeignKey(d => d.IdElement1)
                .HasConstraintName("ratio_of_element_equality_element_of_equality_fk");

            entity.HasOne(d => d.IdElement2Navigation).WithMany(p => p.RatioOfElementEqualityIdElement2Navigations)
                .HasForeignKey(d => d.IdElement2)
                .HasConstraintName("ratio_of_element_equality_element_of_equality_fk_1");
        });

        modelBuilder.Entity<TextOfPutting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("text_of_putting_pk");

            entity.ToTable("text_of_putting");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdQuestion).HasColumnName("id_question");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.TextOfPuttings)
                .HasForeignKey(d => d.IdQuestion)
                .HasConstraintName("text_of_putting_question_fk");
        });

        modelBuilder.Entity<UserResponse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_responses_pk");

            entity.ToTable("user_responses");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Correctly).HasColumnName("correctly");
            entity.Property(e => e.IdQuestion).HasColumnName("id_question");
            entity.Property(e => e.IdSession).HasColumnName("id_session");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.UserResponses)
                .HasForeignKey(d => d.IdQuestion)
                .HasConstraintName("user_responses_question_fk");

            entity.HasOne(d => d.IdSessionNavigation).WithMany(p => p.UserResponses)
                .HasForeignKey(d => d.IdSession)
                .HasConstraintName("user_responses_user_session_fk");
        });

        modelBuilder.Entity<UserResponseArrangement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_response_arrangement_pk");

            entity.ToTable("user_response_arrangement");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdElement).HasColumnName("id_element");
            entity.Property(e => e.IdResponse).HasColumnName("id_response");
            entity.Property(e => e.Position).HasColumnName("position");

            entity.HasOne(d => d.IdElementNavigation).WithMany(p => p.UserResponseArrangements)
                .HasForeignKey(d => d.IdElement)
                .HasConstraintName("user_response_arrangement_element_of_arrangement_fk");

            entity.HasOne(d => d.IdResponseNavigation).WithMany(p => p.UserResponseArrangements)
                .HasForeignKey(d => d.IdResponse)
                .HasConstraintName("user_response_arrangement_user_responses_fk");
        });

        modelBuilder.Entity<UserResponseChooseAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_response_choose_answer_pk");

            entity.ToTable("user_response_choose_answer");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdElement).HasColumnName("id_element");
            entity.Property(e => e.IdResponse).HasColumnName("id_response");

            entity.HasOne(d => d.IdElementNavigation).WithMany(p => p.UserResponseChooseAnswers)
                .HasForeignKey(d => d.IdElement)
                .HasConstraintName("user_response_choose_answer_element_of_choose_fk");

            entity.HasOne(d => d.IdResponseNavigation).WithMany(p => p.UserResponseChooseAnswers)
                .HasForeignKey(d => d.IdResponse)
                .HasConstraintName("user_response_choose_answer_user_responses_fk");
        });

        modelBuilder.Entity<UserResponseMatchTheElement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_response_match_the_element_pk");

            entity.ToTable("user_response_match_the_element");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdElement1).HasColumnName("id_element1");
            entity.Property(e => e.IdElement2).HasColumnName("id_element2");
            entity.Property(e => e.IdResponse).HasColumnName("id_response");

            entity.HasOne(d => d.IdElement1Navigation).WithMany(p => p.UserResponseMatchTheElementIdElement1Navigations)
                .HasForeignKey(d => d.IdElement1)
                .HasConstraintName("user_response_match_the_element_element_of_group_fk");

            entity.HasOne(d => d.IdElement2Navigation).WithMany(p => p.UserResponseMatchTheElementIdElement2Navigations)
                .HasForeignKey(d => d.IdElement2)
                .HasConstraintName("user_response_match_the_element_element_of_group_fk_1");

            entity.HasOne(d => d.IdResponseNavigation).WithMany(p => p.UserResponseMatchTheElements)
                .HasForeignKey(d => d.IdResponse)
                .HasConstraintName("user_response_match_the_element_user_responses_fk");
        });

        modelBuilder.Entity<UserResponseMatchTheValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_response_match_the_value_pk");

            entity.ToTable("user_response_match_the_value");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdElement1).HasColumnName("id_element1");
            entity.Property(e => e.IdElement2).HasColumnName("id_element2");
            entity.Property(e => e.IdResponse).HasColumnName("id_response");

            entity.HasOne(d => d.IdElement1Navigation).WithMany(p => p.UserResponseMatchTheValueIdElement1Navigations)
                .HasForeignKey(d => d.IdElement1)
                .HasConstraintName("user_response_match_the_value_element_of_equality_fk");

            entity.HasOne(d => d.IdElement2Navigation).WithMany(p => p.UserResponseMatchTheValueIdElement2Navigations)
                .HasForeignKey(d => d.IdElement2)
                .HasConstraintName("user_response_match_the_value_element_of_equality_fk_1");

            entity.HasOne(d => d.IdResponseNavigation).WithMany(p => p.UserResponseMatchTheValues)
                .HasForeignKey(d => d.IdResponse)
                .HasConstraintName("user_response_match_the_value_user_responses_fk");
        });

        modelBuilder.Entity<UserResponseMultiplyAnswer>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("user_response_multiply_answer");

            entity.HasIndex(e => e.Id, "user_response_multiply_answer_unique").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdElement).HasColumnName("id_element");
            entity.Property(e => e.IdResponse).HasColumnName("id_response");

            entity.HasOne(d => d.IdElementNavigation).WithMany()
                .HasForeignKey(d => d.IdElement)
                .HasConstraintName("user_response_multiply_answer_element_of_putting_fk");

            entity.HasOne(d => d.IdResponseNavigation).WithMany()
                .HasForeignKey(d => d.IdResponse)
                .HasConstraintName("user_response_multiply_answer_user_responses_fk");
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_session_pk");

            entity.ToTable("user_session");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.UserGroup)
                .HasColumnType("character varying")
                .HasColumnName("user_group");
            entity.Property(e => e.UserName)
                .HasColumnType("character varying")
                .HasColumnName("user_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
