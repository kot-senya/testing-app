using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class Question
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int IdCategory { get; set; }

    public double Price { get; set; }

    public virtual ICollection<ElementOfArrangement> ElementOfArrangements { get; set; } = new List<ElementOfArrangement>();

    public virtual ICollection<ElementOfChoose> ElementOfChooses { get; set; } = new List<ElementOfChoose>();

    public virtual ICollection<ElementOfEquality> ElementOfEqualities { get; set; } = new List<ElementOfEquality>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual Category IdCategoryNavigation { get; set; } = null!;

    public virtual ICollection<QuestionImage> QuestionImages { get; set; } = new List<QuestionImage>();

    public virtual ICollection<QuestionVariant> QuestionVariants { get; set; } = new List<QuestionVariant>();

    public virtual ICollection<TextOfPutting> TextOfPuttings { get; set; } = new List<TextOfPutting>();

    public virtual ICollection<UserResponse> UserResponses { get; set; } = new List<UserResponse>();
}
