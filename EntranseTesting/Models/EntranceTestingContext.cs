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

    public virtual DbSet<AppSetting> AppSettings { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ElementOfArrangement> ElementOfArrangements { get; set; }

    public virtual DbSet<ElementOfChoose> ElementOfChooses { get; set; }

    public virtual DbSet<ElementOfEquality> ElementOfEqualities { get; set; }

    public virtual DbSet<ElementOfGroup> ElementOfGroups { get; set; }

    public virtual DbSet<ElementOfPutting> ElementOfPuttings { get; set; }

    public virtual DbSet<ElementOrientation> ElementOrientations { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<HintImage> HintImages { get; set; }

    public virtual DbSet<HintText> HintTexts { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionHint> QuestionHints { get; set; }

    public virtual DbSet<QuestionImage> QuestionImages { get; set; }

    public virtual DbSet<RatioOfElementEquality> RatioOfElementEqualities { get; set; }

    public virtual DbSet<RootUser> RootUsers { get; set; }

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
        modelBuilder.Entity<AppSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("app_settings_pk");

            entity.ToTable("app_settings");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CountOfHints).HasColumnName("count_of_hints");
            entity.Property(e => e.CountOfQuestions).HasColumnName("count_of_questions");
            entity.Property(e => e.DateOfChanging)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_of_changing");
            entity.Property(e => e.HintVisibility).HasColumnName("hint_visibility");
            entity.Property(e => e.Raiting3).HasColumnName("raiting3");
            entity.Property(e => e.Raiting4).HasColumnName("raiting4");
            entity.Property(e => e.Raiting5).HasColumnName("raiting5");
            entity.Property(e => e.ResultVisibiliry).HasColumnName("result_visibiliry");
            entity.Property(e => e.Time).HasColumnName("time");
        });

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

        modelBuilder.Entity<HintImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("hint_image_pk");

            entity.ToTable("hint_image");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdHint).HasColumnName("id_hint");
            entity.Property(e => e.Image).HasColumnName("image");

            entity.HasOne(d => d.IdHintNavigation).WithMany(p => p.HintImages)
                .HasForeignKey(d => d.IdHint)
                .HasConstraintName("hint_image_hint_text_fk");
        });

        modelBuilder.Entity<HintText>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("hint_text_pk");

            entity.ToTable("hint_text");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Text)
                .HasColumnType("character varying")
                .HasColumnName("text");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("question_pk");

            entity.ToTable("question");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.InTest).HasColumnName("in_test");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Questions)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("question_category_fk");
        });

        modelBuilder.Entity<QuestionHint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("question_hint_pk");

            entity.ToTable("question_hint");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.IdHint).HasColumnName("id_hint");
            entity.Property(e => e.IdQuestion).HasColumnName("id_question");

            entity.HasOne(d => d.IdHintNavigation).WithMany(p => p.QuestionHints)
                .HasForeignKey(d => d.IdHint)
                .HasConstraintName("question_hint_hint_text_fk");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.QuestionHints)
                .HasForeignKey(d => d.IdQuestion)
                .HasConstraintName("question_hint_question_fk");
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

        modelBuilder.Entity<RootUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rooy_users_pk");

            entity.ToTable("root_users");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Login)
                .HasColumnType("character varying")
                .HasColumnName("login");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
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
            entity.Property(e => e.HintApply).HasColumnName("hint_apply");
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
            entity.Property(e => e.Usercorrectly).HasColumnName("usercorrectly");

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
            entity.HasKey(e => e.Id).HasName("user_response_multiply_answer_pk");

            entity.ToTable("user_response_multiply_answer");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdElement).HasColumnName("id_element");
            entity.Property(e => e.IdResponse).HasColumnName("id_response");
            entity.Property(e => e.IdText).HasColumnName("id_text");

            entity.HasOne(d => d.IdElementNavigation).WithMany(p => p.UserResponseMultiplyAnswers)
                .HasForeignKey(d => d.IdElement)
                .HasConstraintName("user_response_multiply_answer_element_of_putting_fk");

            entity.HasOne(d => d.IdResponseNavigation).WithMany(p => p.UserResponseMultiplyAnswers)
                .HasForeignKey(d => d.IdResponse)
                .HasConstraintName("user_response_multiply_answer_user_responses_fk");

            entity.HasOne(d => d.IdTextNavigation).WithMany(p => p.UserResponseMultiplyAnswers)
                .HasForeignKey(d => d.IdText)
                .HasConstraintName("user_response_multiply_answer_text_of_putting_fk");
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_session_pk");

            entity.ToTable("user_session");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CountHint).HasColumnName("count_hint");
            entity.Property(e => e.Date)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.IdAppSettings).HasColumnName("id_app_settings");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.UserGroup)
                .HasColumnType("character varying")
                .HasColumnName("user_group");
            entity.Property(e => e.UserName)
                .HasColumnType("character varying")
                .HasColumnName("user_name");

            entity.HasOne(d => d.IdAppSettingsNavigation).WithMany(p => p.UserSessions)
                .HasForeignKey(d => d.IdAppSettings)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("user_session_app_settings_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
