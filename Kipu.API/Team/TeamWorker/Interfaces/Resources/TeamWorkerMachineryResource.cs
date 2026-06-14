using System.ComponentModel.DataAnnotations;

namespace Kipu.API.Team.TeamWorker.Interfaces.Resources;

public record TeamWorkerMachineryResource(
    [Required(ErrorMessage = "Machinery ID is required.")]
    string MachineryId, 

    [Required(ErrorMessage = "Machinery full name is required.")]
    [MaxLength(200, ErrorMessage = "Machinery full name cannot exceed 200 characters.")]
    string FullName
);