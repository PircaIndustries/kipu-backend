using Kipu.API.Shared.Domain.Model;

namespace Kipu.API.Projects.Domain.Model.Aggregates;

public class Project : IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public double Budget { get; set; }
    public int Progress { get; set; } = 0;
    public string Status { get; set; } = "Planificación";
    public string StatusJustification { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public int Members { get; set; } = 1;
    public int Rnc { get; set; } = 0;
    public int Pending { get; set; } = 0;

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public ICollection<ProjectItem> Items { get; set; } = new List<ProjectItem>();

    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public Project() { }

    public Project(string name, string location, double budget, string description = "", DateTime? startDate = null, DateTime? endDate = null, string image = "", int members = 1, int rnc = 0, int pending = 0, string status = "Planificación", string statusJustification = "Proyecto creado e iniciado.")
    {
        Name = name;
        Location = location;
        Budget = budget;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Image = image;
        Members = members;
        Rnc = rnc;
        Pending = pending;
        Status = status;
        StatusJustification = statusJustification;
    }
}
