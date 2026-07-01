using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Shared.Domain.Model;

namespace Kipu.API.Logistics.Domain.Model.Aggregates;

public partial class MachineryAssignment
{
    protected MachineryAssignment()
    {
        Id = Guid.NewGuid().ToString();
        Name = string.Empty;
        ProjectId = string.Empty;
        MachineryId = string.Empty;
        Status = "AVAILABLE";
        MaintenanceHours = "0";
    }

    public MachineryAssignment(CreateMachineryAssignmentCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        Id = Guid.NewGuid().ToString();
        ProjectId = command.ProjectId;
        MachineryId = command.MachineryId;
        Name = command.Name;
        Status = "AVAILABLE";
        AssignedTo = null;
        AssignedWorkerId = null;
        RegistrationDate = DateTime.UtcNow;
        MaintenanceHours = command.MaintenanceHours ?? "0";
        AssignmentDetail = command.AssignmentDetail ?? string.Empty;
    }

    public void Update(UpdateMachineryAssignmentCommand command)
    {
        if (command.Name is not null) Name = command.Name;
        if (command.Status is not null) Status = command.Status;
        if (command.AssignedTo is not null) AssignedTo = command.AssignedTo;
        if (command.AssignedWorkerId is not null) AssignedWorkerId = command.AssignedWorkerId;
        if (command.MaintenanceHours is not null) MaintenanceHours = command.MaintenanceHours;
        if (command.AssignmentDetail is not null) AssignmentDetail = command.AssignmentDetail;
    }

    public string Id { get; private set; }
    public string ProjectId { get; private set; }
    public string MachineryId { get; private set; }
    public string Name { get; private set; }
    public string Status { get; private set; }
    public string? AssignedTo { get; private set; }
    public string? AssignedWorkerId { get; private set; }
    public DateTime RegistrationDate { get; private set; }
    public string MaintenanceHours { get; private set; }
    public string AssignmentDetail { get; private set; }
}
