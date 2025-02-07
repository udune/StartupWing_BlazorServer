using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using StartupWing_BlazorServer.Components.Datas;
using StartupWing_BlazorServer.Components.Modals;
using StartupWing_BlazorServer.Components.Templates;

namespace StartupWing_BlazorServer.Components.Pages;

public partial class EmployeeManage : ComponentBase
{
    List<UserData>? _apiDatas = new();
    List<UserData>? _pageDatas = new();
    List<UserData>? _tableDatas = new();
    
    private int _currentPage = 1;
    private int _pageSize = 10;
    
    UserData? _selectEmployeeData = new ();
    UserData _modifyEmployeeData = new ();
    UserData? _myData = new ();
    
    private Modal addEmployee;
    private Modal modifyEmployee;
    private Modal popup;

    private SearchTemplate.DataSort currentSortData = SearchTemplate.DataSort.Descending;
    private TableTemplate<UserData> table;
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (DataManager.MyData != null)
            {
                _myData = DataManager.MyData;
                await GetMyUserDatas(_myData.OrganizationId);
            }
        }
    }

    private async Task GetMyUserDatas(int? myOrganizationId)
    {
        _apiDatas = await APIService.GetEmployeeData_OrganizationID(myOrganizationId);
        _tableDatas = _apiDatas;
        UpdatePageData(_currentPage);
    }
    
    private void UpdatePageData(int currentPage)
    {
        _currentPage = currentPage;
        _pageDatas = _apiDatas?.Skip((currentPage - 1) * _pageSize).Take(_pageSize).ToList();
        _pageDatas = currentSortData.Equals(SearchTemplate.DataSort.Ascending) ? _pageDatas?.OrderBy(item => item.CreatedTime).ToList() : _pageDatas?.OrderByDescending(item => item.CreatedTime).ToList();
        StateHasChanged();
    }
    
    private void SortByAscending()
    {
        currentSortData = SearchTemplate.DataSort.Ascending;
        _pageDatas = _pageDatas?.OrderBy(item => item.CreatedTime).ToList();
        if (_pageDatas != null) table.TableUpdate(_pageDatas);
    }

    private void SortByDescending()
    {
        currentSortData = SearchTemplate.DataSort.Descending;
        _pageDatas = _pageDatas?.OrderByDescending(item => item.CreatedTime).ToList();
        if (_pageDatas != null) table.TableUpdate(_pageDatas);
    }

    #region Search

    private void OnChange(string searching)
    {
        if (string.IsNullOrWhiteSpace(searching))
        {
            UpdatePageData(_currentPage);
            _tableDatas = _apiDatas;
            if (_pageDatas != null) table.TableUpdate(_pageDatas, _tableDatas);
        }
    }

    private void OnSearch(KeyboardEventArgs e, string searching)
    {
        if (e.Key == "Enter")
        {
            if (!string.IsNullOrWhiteSpace(searching))
            {
                _pageDatas = _apiDatas?.Where(item => (item.Name ?? "").ToString().Contains(searching) ||
                                                      (item.Email ?? "").Contains(searching, StringComparison.OrdinalIgnoreCase) ||
                                                      (item.Department ?? "").Contains(searching, StringComparison.OrdinalIgnoreCase) ||
                                                      (item.Position ?? "").Contains(searching, StringComparison.OrdinalIgnoreCase) ||
                                                      (item.Nickname ?? "").ToString().Contains(searching, StringComparison.OrdinalIgnoreCase)
                ).Take(_pageSize).ToList();
                _tableDatas = _pageDatas;
                if (_pageDatas != null) table.TableUpdate(_pageDatas, _tableDatas);
            }
            else
            {
                UpdatePageData(_currentPage); // 검색어가 없을 경우 모든 데이터 표시
                _tableDatas = _apiDatas;
                if (_pageDatas != null) table.TableUpdate(_pageDatas, _tableDatas);
            }
        }
    }

    #endregion
    
    #region 직원 수정

    private void ModifyUser(UserData userData)
    {
        _selectEmployeeData = userData;
        
        if (_myData != null)
        {
            // 내 권한이 관리자 일 경우
            if (_myData.Role == "A" || _myData.Role == "C")
            {
                OpenModifyUser(userData);
            }
            else // 내 권한이 일반 일 경우
            {
                if (_myData.Id == _selectEmployeeData.Id)
                {
                    OpenModifyUser(userData);
                }
            }
        }

        _selectEmployeeData = null;
        StateHasChanged();
    }

    private void OpenModifyUser(UserData userData)
    {
        _modifyEmployeeData = new UserData
        {
            Id = userData.Id,
            Name = userData.Name,
            Email = userData.Email,
            Nickname = userData.Nickname,
            Department = userData.Department,
            Position = userData.Position,
            PhoneNumber = userData.PhoneNumber,
            CreatedTime = userData.CreatedTime,
            Role = userData.Role,
            Address = userData.Address,
            AccountNumber = userData.AccountNumber,
            OrganizationId = userData.OrganizationId
        };

        modifyEmployee.Show<ModifyEmployeeModal>(new Dictionary<string, object?>
        {
            {"ModifyEmployeeData", _modifyEmployeeData},
            {"MyRole", _myData?.Role},
            {"ModifySubmitAction", EventCallback.Factory.Create(this, OnModifySubmit)},
            {"OnRemoveSubmitAction", EventCallback.Factory.Create(this, OnRemoveSubmit)}
        }, true);
    }
    
    #endregion

    #region 직원 초대

    private void AddUser()
    {
        addEmployee.Show<AddEmployeeModal>(null, true);
    }

    #endregion

    #region ## TemplateForm Callback

    async Task OnModifySubmit()
    {
        if (string.IsNullOrEmpty(_modifyEmployeeData.Name) || string.IsNullOrEmpty(_modifyEmployeeData.Email))
            return;
        
        await ModifyTable();
    }

    Task OnRemoveSubmit()
    {
        popup.Show<Popup>("알림", "삭제하시겠어요?", 2, async () =>
        {
            await RemoveTable();
        });
        return Task.CompletedTask;
    }

    private async Task ModifyTable()
    {
        var result = await APIService.UpdateEmployeeData(_modifyEmployeeData);
        if (result)
        {
            _apiDatas = await APIService.GetEmployeeData_OrganizationID(_myData?.OrganizationId);
            if (_apiDatas != null)
            {
                UpdatePageData(_currentPage);
            }
        }
    }

    private async Task RemoveTable()
    {
        var result = await APIService.RemoveEmployeeData(_modifyEmployeeData);
        if (result)
        {
            _apiDatas = await APIService.GetEmployeeData_OrganizationID(_myData?.OrganizationId);
            if (_apiDatas != null)
            {
                UpdatePageData(_currentPage);
            }
        }
    }

    #endregion
}