namespace WebApi.Models.Labels;

using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
 
public class CreateLabelRequest
{
    [Required]
    public string? Name { get; set; }
     
    public string? City { get; set; }
     
    public string? State { get; set; } 
}

public class UpdateLabelRequest
{
    public string? Name { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }
}

public class SearchLabelRequest
{
    public string? Ids { get; set; }
    public string? NameLike { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }
}

public class LabelModel
{
    public int id { get; set; }
    public string name { get; set; }
    public string? city { get; set; }
    public string? state { get; set; }

    LabelModel(int id, string name)
    {
        this.name = name;
        this.id = id;
    }
}