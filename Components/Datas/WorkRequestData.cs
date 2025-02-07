
namespace StartupWing_BlazorServer.Components.Datas;

public class WorkRequestData
{
    public int Id { get; set; }
    public int ConciergeId { get; set; }

    public DateOnly StartDate { get; set; }
    public string? Contents { get; set; }
    public string? ConciergeName { get; set; }

    public string? ReceiveResult { get; set; }

    public string? ApplyType { get; set; }

    public string? Status { get; set; } 

    public DateOnly EndDate { get; set; }

    public DateTime CreateDate { get; set; }
}