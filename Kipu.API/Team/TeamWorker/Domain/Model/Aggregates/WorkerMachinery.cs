namespace Kipu.API.Team.TeamWorker.Domain.Model.Aggregates;

public class WorkerMachinery
{
    public int Id { get; private set; } 
    
    public string MachineryId { get; private set; } 
    
    public string FullName { get; private set; } 

    protected WorkerMachinery() 
    { 
        MachineryId = string.Empty;
        FullName = string.Empty;
    }

    public WorkerMachinery(string machineryId, string fullName)
    {
        MachineryId = machineryId;
        FullName = fullName;
    }
}