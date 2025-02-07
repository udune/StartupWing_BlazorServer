using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace StartupWing_BlazorServer.Components.Modals;

public partial class AddEmployeeModal : ComponentBase
{
    private bool IsAddEmployeeOpen { get; set; } = true;
    private bool IsAddSuccessOpen { get; set; }
    private List<string> EmailList { get; set; } = new();
    private string? CurrentEmail { get; set; } = string.Empty;
    
    private void RemoveEmail(string email)
    {
        EmailList.Remove(email);
    }
    
    private async Task SubmitEmails()
    {
        if (!string.IsNullOrWhiteSpace(CurrentEmail) && !EmailList.Contains(CurrentEmail.Trim()) && IsValidEmail(CurrentEmail))
        {
            EmailList.Add(CurrentEmail.Trim());
        }

        foreach (var email in EmailList)
        {
            Console.WriteLine(email);
        }

        await RequestInviteUser(EmailList);

        EmailList.Clear();
        CurrentEmail = string.Empty;
    }
    
    private bool IsValidEmail(string? email)
    {
        string emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email.Trim(), emailRegex);
    }
    
    private async Task RequestInviteUser(List<string> emailList)
    {
        var requestBody = new
        {
            userList = emailList,
        };

        await SwingServerApiService.SendInviteRequest(DataManager.MyAccessToken, requestBody, () =>
        {
            IsAddSuccessOpen = true;
            IsAddEmployeeOpen = false;
            InvokeAsync(StateHasChanged);
        });
    }
    
    private void HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            var trimmedEmail = CurrentEmail?.Trim();
            if (!string.IsNullOrEmpty(trimmedEmail) && !EmailList.Contains(trimmedEmail) && IsValidEmail(CurrentEmail))
            {
                EmailList.Add(trimmedEmail);
                CurrentEmail = string.Empty;
                InvokeAsync(StateHasChanged);
            }
        }
    }
    
    private void CloseAddSuccessModal()
    {
        IsAddSuccessOpen = false;
        IsAddEmployeeOpen = true;
    }
}