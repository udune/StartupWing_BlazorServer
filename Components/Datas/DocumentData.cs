using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace StartupWing_BlazorServer.Components.Datas;

public class DocumentData
{
    public int Id { get; set; }
    
    public int? OrganizationId { get; set; }
    
    [Required(ErrorMessage = "문서명을 입력해주세요.")]
    public string? Title { get; set; }
    
    [Required(ErrorMessage = "관리번호를 입력해주세요.")]
    public string? ManageNumber { get; set; }
    
    public List<DocumentFileData>? FileData { get; set; }
    public string File
    {
        set => FileData = JsonConvert.DeserializeObject<List<DocumentFileData>>(value);
    }

    [DataType(DataType.Date, ErrorMessage = "날짜 형식이 아닙니다.")]
    public DateTime CreatedTime { get; set; } = DateTime.Now;
    
    [DataType(DataType.Date, ErrorMessage = "날짜 형식이 아닙니다.")]
    public DateTime ExpireAt { get; set; } = DateTime.Now;
    
    public bool IsPublic { get; set; }
    
    public string? Note { get; set; }
}

public class DocumentFileData
{
    public string Url { get; set; }
    public string FileName { get; set; }
}