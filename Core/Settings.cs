namespace Core;

public record Settings {
    public required Dictionary<string, string> SecretKeys {get;set;}
    public required Dictionary<string, string> ClientIds {get;set;} 
}