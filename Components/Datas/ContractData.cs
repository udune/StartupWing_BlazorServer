using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace StartupWing_BlazorServer.Components.Datas;

public class ContractData
{
    public int ContractId { get; set; }
    
    public int? OrganizationId { get; set; }
    
    public int? CategoryId { get; set; }
    
    [Required(ErrorMessage = "관리번호를 입력해주세요.")]
    public string? ManageNumber { get; set; }
    
    [Required(ErrorMessage = "문서명을 입력해주세요.")]
    public string? Title { get; set; }
    
    public string? Offeree { get; set; }
    
    [DataType(DataType.Date, ErrorMessage = "날짜 형식이 아닙니다.")]
    public DateTime ContractDate { get; set; } = DateTime.Now;
    
    public string? Term { get; set; }
    
    public string? Notes { get; set; }
    
    public List<ContractFileData>? FileData { get; set; }
    public string File
    {
        set => FileData = JsonConvert.DeserializeObject<List<ContractFileData>>(value);
    }
    
    public bool IsPublic { get; set; }
}

public class ContractFileData
{
    public string Url { get; set; }
    public string FileName { get; set; }
}