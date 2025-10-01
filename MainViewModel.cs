using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AIAssistant;

internal partial class MainViewModel : ObservableObject
{
    HttpClient _httpClient = new HttpClient()
    {
        BaseAddress = new Uri("https://localhost:7109"),
        Timeout = TimeSpan.FromSeconds(30)
    };

    [ObservableProperty]
    string? _message;

    [ObservableProperty]
    string? _answer;

    [RelayCommand]
    async Task SendMessageAsync()
    {
        Answer = string.Empty;
        try
        {
            using (
                var response = await _httpClient.GetAsync(
                    $"question?q='{Uri.EscapeDataString(Message ?? "")}'",
                    HttpCompletionOption.ResponseHeadersRead
                )
            )
            {
                response.EnsureSuccessStatusCode();
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;
                    while ((bytesRead = await stream.ReadAsync(buffer, 0, 1024)) > 0)
                    {
                        Answer += Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Answer = ex.Message;
        }
    }

    bool CanSendMessage() => !string.IsNullOrEmpty(Message);
}
