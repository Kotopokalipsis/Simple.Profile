namespace Infrastructure.Options;

public record RabbitMqOptions()
{
    public const string SectionPath = "RabbitMq";
    public string Hostname { get; set; }
    public string Queue { get; set; }
};