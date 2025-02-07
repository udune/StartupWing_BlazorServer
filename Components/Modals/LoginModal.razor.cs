using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

namespace StartupWing_BlazorServer.Components.Modals;

public class LoginInfo
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string password { get; set; }
}

public partial class LoginModal : ComponentBase
{
    private LoginInfo loginInfo = new();
    
    private async Task RequestLogin()
    {
        var ok = await APIService.LoginUserData(loginInfo, (result) =>
        {
            DataManager.MyData = result;
            Modal.Close();
            Navigation.NavigateTo(Navigation.Uri, false);
        });
    }
}