namespace Kipu.API.Team.TeamUser.domain.model.ValueObjects;

public record Email
{
    public string Address { get; }

    public Email(string address)
    {
        if (string.IsNullOrWhiteSpace(address)) 
            throw new ArgumentException("Email cannot be empty");
        
        if (!address.Contains("@")) 
            throw new ArgumentException("Invalid email format");
            
        Address = address;
    }
}