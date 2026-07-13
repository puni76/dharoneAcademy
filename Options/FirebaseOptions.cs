namespace DharoneAcademy.Options;

public class FirebaseOptions
{
    public const string SectionName = "Firebase";

    public string ProjectId { get; set; } = "dharoneacademy";
    public string ConsoleUrl { get; set; } = "https://console.firebase.google.com/project/dharoneacademy/";
    public bool AnalyticsEnabled { get; set; }
    public string? ApiKey { get; set; }
    public string? AuthDomain { get; set; }
    public string? StorageBucket { get; set; }
    public string? MessagingSenderId { get; set; }
    public string? AppId { get; set; }
    public string? MeasurementId { get; set; }

    public bool IsAnalyticsConfigured =>
        AnalyticsEnabled
        && !string.IsNullOrWhiteSpace(ApiKey)
        && !string.IsNullOrWhiteSpace(AppId)
        && !string.IsNullOrWhiteSpace(MeasurementId)
        && !ApiKey.Contains("YOUR_", StringComparison.OrdinalIgnoreCase)
        && !AppId.Contains("YOUR_", StringComparison.OrdinalIgnoreCase);
}
