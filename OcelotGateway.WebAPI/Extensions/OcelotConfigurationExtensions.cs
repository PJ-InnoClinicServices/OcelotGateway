using System.Text;
using Newtonsoft.Json.Linq;

namespace OcelotGateway;

public static class OcelotConfigurationExtensions
{
    public static IConfigurationBuilder AddOcelotRoutes(this IConfigurationBuilder builder, string mainConfigPath = "ocelot.json", string routesFolderPath = "./ocelot-config")
    {
        var ocelotConfig = JObject.Parse(File.ReadAllText(mainConfigPath));
        
        if (Directory.Exists(routesFolderPath))
        {
            var configFiles = Directory.GetFiles(routesFolderPath, "*.json");

            foreach (var file in configFiles)
            {
                var json = JObject.Parse(File.ReadAllText(file));

                if (json["Routes"] is JArray newRoutes && ocelotConfig["Routes"] is JArray existingRoutes)
                {
                    foreach (var route in newRoutes)
                    {
                        existingRoutes.Add(route);
                    }
                }
            }
        }
        
        var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(ocelotConfig.ToString()));
        builder.AddJsonStream(memoryStream);
        return builder;
    }
}