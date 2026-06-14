using Kipu.API.Team.TeamWorker.Domain.Model.ValueObjects;

namespace Kipu.API.Team.TeamWorker.Domain.Model.Aggregates;

public partial class TeamWorker
{
    public WorkerId Id { get; private set; }
    public string Dni { get; private set; }
    public string FullName { get; private set; }
    public string Role { get; private set; }
    public bool IsActive { get; private set; }
    public string ProjectId { get; private set; }
    
    private readonly List<WorkerMachinery> _machineries = new();
    public IReadOnlyCollection<WorkerMachinery> Machineries => _machineries.AsReadOnly();
    
    protected TeamWorker() 
    { 
        Dni = string.Empty;
        FullName = string.Empty;
        Role = string.Empty;
        ProjectId = string.Empty;
    }

    public TeamWorker(string dni, string fullName, string role, string projectId)
    {
        Id = new WorkerId(); 
        Dni = dni;
        FullName = fullName;
        Role = role;
        ProjectId = projectId;
        IsActive = true; 
    }


    public void AssignMachinery(string machineryId, string fullName)
    {
        if (_machineries.Any(m => m.MachineryId == machineryId))
            throw new InvalidOperationException("This machinery is already assigned.");

        _machineries.Add(new WorkerMachinery(machineryId, fullName));
    }

    public void RemoveMachinery(string machineryId)
    {
        var machinery = _machineries.FirstOrDefault(m => m.MachineryId == machineryId);
        if (machinery != null)
        {
            _machineries.Remove(machinery);
        }
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}