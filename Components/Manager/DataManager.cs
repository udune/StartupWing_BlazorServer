namespace StartupWing_BlazorServer.Components.Manager;
using Datas;

public class DataManager
{
    public UserData? MyData;
    public string MyAccessToken { get; set; } = string.Empty;
    public bool Hidden { get; set; }
}