using Microsoft.AspNetCore.Components;

namespace StartupWing_BlazorServer.Components.Modals;

public partial class AddContractModal : ComponentBase
{
    private const string GENERAL_CONTRACT = "일반 계약";
    private const string PRODUCT_CONTRACT = "제품 공급 계약";
    private const string SERVICE_CONTRACT = "용역 계약";
    
    private string CurrentContractTypeText = GENERAL_CONTRACT;
    
    bool isDropdownOpen;
    
    private void OnAddSubmit()
    {
        AddSubmitAction.InvokeAsync();
        Modal.Close();
    }
    
    private void OnChangeIsPublic(ChangeEventArgs e)
    {
        if (e.Value?.ToString() == "open")
        {
            AddContractData.IsPublic = true;
        }
        else if (e.Value?.ToString() == "close")
        {
            AddContractData.IsPublic = false;
        }
    }
    
    private void DropdownOptionEvent(ChangeEventArgs e)
    {
        Console.WriteLine(e.Value);
        CurrentContractTypeText = e.Value?.ToString() ?? "";
        if(isDropdownOpen)
            isDropdownOpen = false;
        StateHasChanged();
    }
    
    private void OnCancel()
    {
        Modal.Close();
    }
}