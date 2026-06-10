namespace Kipu.API.Projects.Domain.Model.ValueObjects;

public static class ProjectStatus
{
    public const string Planificacion = "Planificación";
    public const string EnEjecucion = "En ejecución";
    public const string Detenido = "Detenido";
    public const string Paralizada = "Paralizada";
    public const string Completada = "Completada";
    public const string Finalizada = "Finalizada";

    public static readonly List<string> All =
    [
        Planificacion,
        EnEjecucion,
        Detenido,
        Paralizada,
        Completada,
        Finalizada
    ];

    public static bool IsValid(string status)
    {
        return All.Contains(status);
    }
}
