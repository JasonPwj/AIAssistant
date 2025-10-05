using CommunityToolkit.Mvvm.ComponentModel;

namespace AIAssistant.Models;

internal partial class QuestionAndAnswer : ObservableObject
{
    [ObservableProperty]
    string _content = string.Empty;

    [ObservableProperty]
    bool _isQuestion = false;
}
