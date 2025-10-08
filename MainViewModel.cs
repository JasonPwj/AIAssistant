using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using AIAssistant.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AIAssistant;

internal partial class MainViewModel : ObservableObject
{
    HttpClient _httpClient = new HttpClient()
    {
        //BaseAddress = new Uri(Preferences.Get("ServerAddr", "http://192.168.0.97:5043")),
        BaseAddress = new Uri("http://192.168.0.97:5043"),
        Timeout = TimeSpan.FromSeconds(30)
    };

    [ObservableProperty]
    string? _message;

    [ObservableProperty]
    ObservableCollection<QuestionAndAnswer> _chats = new ObservableCollection<QuestionAndAnswer>();

    [RelayCommand]
    async Task SendMessageAsync()
    {
        if (string.IsNullOrEmpty(Message))
            return;
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            return;
        }
        Chats.Add(new QuestionAndAnswer { Content = Message, IsQuestion = true });
        try
        {
            using (
                var response = await _httpClient.GetAsync(
                    $"question?q='{Uri.EscapeDataString(Message)}'",
                    HttpCompletionOption.ResponseHeadersRead
                )
            )
            {
                response.EnsureSuccessStatusCode();
                var newAnswer = new QuestionAndAnswer
                {
                    Content = string.Empty,
                    IsQuestion = false
                };
                Chats.Add(newAnswer);
                Message = string.Empty;
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;
                    while ((bytesRead = await stream.ReadAsync(buffer, 0, 1024)) > 0)
                    {
                        newAnswer.Content += Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    bool CanSendMessage() => !string.IsNullOrEmpty(Message);
}
