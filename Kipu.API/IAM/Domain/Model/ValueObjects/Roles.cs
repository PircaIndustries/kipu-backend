namespace Kipu.API.IAM.Domain.Model.ValueObjects;

public static class Roles
{
    public const string Administrador = "Administrador";
    public const string GestorOperativo = "Gestor Operativo";
    public const string LogisticaYAdministracion = "Logística y Administración";
    public const string Cliente = "Cliente";
    public const string Ingeniero = "Ingeniero";

    public static readonly List<string> All =
    [
        Administrador,
        GestorOperativo,
        LogisticaYAdministracion,
        Cliente,
        Ingeniero
    ];

    public static bool IsValid(string role)
    {
        return All.Contains(role);
    }
}
