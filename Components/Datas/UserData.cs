using System.ComponentModel.DataAnnotations;

namespace StartupWing_BlazorServer.Components.Datas;

public class UserData
{
    public int Id { get; set; } = 0;
    
    [Required(ErrorMessage = "이름을 입력해주세요.")]
    // [StringLength(10, MinimumLength = 2)]
    public string Name { get; set; } = "";
    
    [Required(ErrorMessage = "이메일 주소를 입력해주세요.")]
    [DataType(DataType.EmailAddress, ErrorMessage = "이메일 형식이 아닙니다.")]
    public string Email { get; set; } = "";
    
    public string Nickname { get; set; } = "";
    
    public string Department { get; set; } = "";
    
    public string Position { get; set; } = "";
    
    [DataType(DataType.PhoneNumber, ErrorMessage = "전화번호 형식이 아닙니다.")]
    public string PhoneNumber { get; set; } = "";
    
    public string Address { get; set; } = "";
    
    public string AccountNumber { get; set; } = "";
    
    [DataType(DataType.Date, ErrorMessage = "날짜 형식이 아닙니다.")]
    public DateTime CreatedTime { get; set; } = DateTime.Now;
    
    public string? Role { get; set; } = "M";
    
    public int? OrganizationId = 0;
    public OrganizationData? Organization { get; set; } = new ();
}

public class OrganizationData
{
    public string? Name { get; set; } = "";
    public string RegistrationNumber { get; set; } = "";
    public string Contact { get; set; } = "";
}