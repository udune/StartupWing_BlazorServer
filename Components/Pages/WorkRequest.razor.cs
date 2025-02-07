using Microsoft.AspNetCore.Components;

namespace StartupWing_BlazorServer.Components.Pages;

public partial class WorkRequest : ComponentBase
{
    private bool IsCertificateSelected { get; set; }
    private bool IsIncomeProofSelected { get; set; }
    private bool IsCareerProofSelected { get; set; }
    private bool IsOtherSelected { get; set; }
    private string GeneralRequestContent { get; set; } = string.Empty;

    private void SubmitRequest()
    {
        var selectedDocuments = new List<string>();
        if (IsCertificateSelected) selectedDocuments.Add("재직증명서");
        if (IsIncomeProofSelected) selectedDocuments.Add("원천징수영수증");
        if (IsCareerProofSelected) selectedDocuments.Add("경력증명서");
        if (IsOtherSelected) selectedDocuments.Add("기타");

        if (string.IsNullOrEmpty(GeneralRequestContent))
            return;

        Console.WriteLine("선택된 문서:");
        foreach (var doc in selectedDocuments)
        {
            Console.WriteLine(doc);
        }
        Console.WriteLine($"일반 요청 내용: {GeneralRequestContent}");
    }

    private void ToggleAllSelection()
    {
        bool newState = !(IsCertificateSelected && IsIncomeProofSelected && IsCareerProofSelected && IsOtherSelected);
        IsCertificateSelected = newState;
        IsIncomeProofSelected = newState;
        IsCareerProofSelected = newState;
        IsOtherSelected = newState;
    }

    private void UpdateCharacterCount(ChangeEventArgs e)
    {
        GeneralRequestContent = e.Value?.ToString();
    }
}