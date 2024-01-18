using Microsoft.Extensions.DependencyInjection;

namespace G4di.Generator;
internal class RegexInjectionDefinitoion
{
    public ServiceLifetime ServiceLifetime { get; set; }
    public string RegexPattern { get; set; } = null!;
}
