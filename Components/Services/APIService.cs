using System.Text;
using Newtonsoft.Json;
using StartupWing_BlazorServer.Components.Datas;
using StartupWing_BlazorServer.Components.Modals;

namespace StartupWing_BlazorServer.Components.Services;

public class APIService (HttpClient client)
{
    // todo API 함수 하나로 통일할 필요
    
    public async Task<bool> LoginUserData(LoginInfo user, Action<UserData?> onSuccess, Action onFail = null)
    {
        var jsonStr = JsonConvert.SerializeObject(user);
        var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("User/Login", content);
        
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Success: {response.StatusCode}, {result}");
            
            var userdata = JsonConvert.DeserializeObject<UserData>(result);
            onSuccess?.Invoke(userdata);
        }
        else
        {
            Console.WriteLine($"Failed: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            onFail?.Invoke();
        }

        return true;
    }
    
    public async Task<UserData?> GetUserData_ID(int? id)
    {
        var result = await client.GetAsync($"User/GetUser/id/{id}");

        var resultContent = await result.Content.ReadAsStringAsync();
        var resultEmployeeData = JsonConvert.DeserializeObject<UserData>(resultContent);
        return resultEmployeeData;
    }
    
    public async Task<List<ContractData>?> GetContractData_OrganizationID(int? organizationId)
    {
        var result = await client.GetAsync($"Contract/GetContract/organizationid/{organizationId}");
        var resultContent = await result.Content.ReadAsStringAsync();
        var resultDocumentData = JsonConvert.DeserializeObject<List<ContractData>>(resultContent);
        return resultDocumentData;
    }

    public async Task<List<DocumentData>?> GetDocumentData_OrganizationID(int? organizationId)
    {
        var result = await client.GetAsync($"Document/GetDocument/organizationid/{organizationId}");
        var resultContent = await result.Content.ReadAsStringAsync();
        var resultDocumentData = JsonConvert.DeserializeObject<List<DocumentData>>(resultContent);
        return resultDocumentData;
    }
    
    public async Task<UserData?> AddEmployeeData(UserData user)
    {
        var jsonStr = JsonConvert.SerializeObject(user);
        var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        var result = await client.PostAsync("EmployeeManage/AddEmployee", content);

        if (result.IsSuccessStatusCode == false)
            throw new Exception("AddEmployee is Failed");

        var resultContent = await result.Content.ReadAsStringAsync();
        var resultEmployeeData = JsonConvert.DeserializeObject<UserData>(resultContent);
        return resultEmployeeData;
    }

    public async Task<UserData?> GetEmployeeData_ID(int? id)
    {
        var result = await client.GetAsync($"EmployeeManage/GetEmployee/id/{id}");

        var resultContent = await result.Content.ReadAsStringAsync();
        var resultEmployeeData = JsonConvert.DeserializeObject<UserData>(resultContent);
        return resultEmployeeData;
    }

    public async Task<List<UserData>?> GetEmployeeData_OrganizationID(int? organizationId)
    {
        var result = await client.GetAsync($"EmployeeManage/GetEmployee/organizationid/{organizationId}");

        var resultContent = await result.Content.ReadAsStringAsync();
        var resultEmployeeData = JsonConvert.DeserializeObject<List<UserData>>(resultContent);
        return resultEmployeeData;
    }

    public async Task<bool> UpdateEmployeeData(UserData user)
    {
        var jsonStr = JsonConvert.SerializeObject(user);
        var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
        var result = await client.PutAsync("EmployeeManage/UpdateEmployee", content);

        if (result.IsSuccessStatusCode == false)
            throw new Exception("UpdateEmployeeData Failed");

        return true;
    }

    public async Task<bool> RemoveEmployeeData(UserData userData)
    {
        var result = await client.DeleteAsync($"EmployeeManage/RemoveEmployee/{userData.Id}");

        if (result.IsSuccessStatusCode == false)
            throw new Exception("RemoveEmployeeData Failed");

        return true;
    }
}