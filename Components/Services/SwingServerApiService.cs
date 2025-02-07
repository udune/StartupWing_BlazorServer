using StartupWing_BlazorServer.Components.Manager;

namespace StartupWing_BlazorServer.Components.Services;

public class SwingServerApiService(HttpClient httpClient)
{
    public async Task SendInviteRequest(string token, object requestBody, Action onSuccess, Action onFail = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/user/user/invite");
        request.Headers.Add("Cookie", $"accessToken={token}");
        request.Content = JsonContent.Create(requestBody);

        var response = await httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Success: {response.StatusCode}, {result}");
            onSuccess?.Invoke();
        }
        else
        {
            Console.WriteLine($"Failed: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            onFail?.Invoke();
        }
    }
}