using Newtonsoft.Json.Linq;

namespace OcelotGateway;
public abstract class PriceUpdateHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        
        // These handler reduces price by half, gets rid of Id and Substances filed that are redundant
        var response = await base.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode && response.Content.Headers.ContentType.MediaType == "application/json")
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonArray = JArray.Parse(responseBody);
            
            foreach (var item in jsonArray)
            {
                var jsonObject = (JObject)item;
                if (jsonObject["price"] != null)
                {
                    var price = jsonObject.Value<decimal>("price");
                    jsonObject["price"] = price / 2;
                }
                jsonObject.Property("id")?.Remove();     
                jsonObject.Property("substances")?.Remove(); 
            }
            response.Content = new StringContent(jsonArray.ToString(), System.Text.Encoding.UTF8, "application/json");
        }
        return response;
    }
}