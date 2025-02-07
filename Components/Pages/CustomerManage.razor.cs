using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using StartupWing_BlazorServer.Components.Datas;
using StartupWing_BlazorServer.Components.Modals;
using StartupWing_BlazorServer.Components.Templates;

namespace StartupWing_BlazorServer.Components.Pages;

public partial class CustomerManage : ComponentBase
{
    enum ContractType
    {
        general,
        product,
        service
    }

    enum DisclosureType
    {
        Full,
        Confidential,
        TopSecret,
        SecondSecret,
    }    
    
    ContractType CurrentSelected = ContractType.general;
    private const string GENERAL_CONTRACT = "일반 계약";
    private const string PRODUCT_CONTRACT = "제품 공급 계약";
    private const string SERVICE_CONTRACT = "용역 계약";
    private ContractType CurrentContractType = ContractType.general;
    private string CurrentContractTypeText = GENERAL_CONTRACT;

    List<ContractData>? _apiDatas = new();
    List<ContractData>? _pageDatas = new();
    List<ContractData>? _tableDatas = new();

    private int _currentPage = 1;
    private int _pageSize = 10;
    
    ContractData? _selectContractData = new ();
    ContractData _addContractData = new ();
    UserData? _myData = new ();
    
    private Modal addContract;
    private Modal popup;
    
    private SearchTemplate.DataSort currentSortData = SearchTemplate.DataSort.Descending;
    private TableTemplate<ContractData> table;
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (DataManager.MyData != null)
            {
                _myData = DataManager.MyData;
                await GetMyContractDatas(_myData.OrganizationId);
            }
        }
    }
    
    private async Task GetMyContractDatas(int? myOrganizationId)
    {
        _apiDatas = await APIService.GetContractData_OrganizationID(myOrganizationId);
        _tableDatas = _apiDatas;
        UpdatePageData(_currentPage);
    }
    
    private void SelectButton(ContractType button)
    {
        CurrentSelected = button;
        switch(button)
        {
            case ContractType.general:
                CurrentContractType = ContractType.general;
                CurrentContractTypeText = GENERAL_CONTRACT;
                break;
            case ContractType.product:
                CurrentContractType = ContractType.product;
                CurrentContractTypeText = PRODUCT_CONTRACT;
                break;
            case ContractType.service:
                CurrentContractType = ContractType.service;
                CurrentContractTypeText = SERVICE_CONTRACT;
                break;
        }
        UpdatePageData(_currentPage);
    }
    
    private string GetContractButtonClass(ContractType button)
    {
        return button == CurrentSelected ? "contractButton selected" : "contractButton";
    }
    
    private void UpdatePageData(int currentPage)
    {
        _currentPage = currentPage;
        _pageDatas = _apiDatas?.Skip((currentPage - 1) * _pageSize).Take(_pageSize).ToList();
        _pageDatas = currentSortData.Equals(SearchTemplate.DataSort.Ascending) ? _pageDatas?.OrderBy(item => item.ContractDate).ToList() : _pageDatas?.OrderByDescending(item => item.ContractDate).ToList();
        StateHasChanged();
    }
    
    private void SortByAscending()
    {
        currentSortData = SearchTemplate.DataSort.Ascending;
        _pageDatas = _pageDatas?.OrderBy(item => item.ContractDate).ToList();
        if (_pageDatas != null) table.TableUpdate(_pageDatas);
    }

    private void SortByDescending()
    {
        currentSortData = SearchTemplate.DataSort.Descending;
        _pageDatas = _pageDatas?.OrderByDescending(item => item.ContractDate).ToList();
        if (_pageDatas != null) table.TableUpdate(_pageDatas);
    }
    
    private void OnChange(string searching)
    {
        if (string.IsNullOrWhiteSpace(searching))
        {
            UpdatePageData(_currentPage);
            _tableDatas = _apiDatas;
            if (_pageDatas != null) table.TableUpdate(_pageDatas, _tableDatas);
        }
    }

    void OnSearch(KeyboardEventArgs e, string searching)
    {
        if (e.Key.Equals("Enter"))
        {
            if (!string.IsNullOrWhiteSpace(searching))
            {
                _pageDatas = _apiDatas?.Where(item => (item.ManageNumber ?? "").Contains(searching, StringComparison.OrdinalIgnoreCase) ||
                                                      (item.Title ?? "").Contains(searching, StringComparison.OrdinalIgnoreCase) ||
                                                      (item.FileData[0].FileName ?? "").Contains(searching, StringComparison.OrdinalIgnoreCase) ||
                                                      (item.Offeree ?? "").Contains(searching, StringComparison.OrdinalIgnoreCase) ||
                                                      (item.Notes ?? "").Contains(searching, StringComparison.OrdinalIgnoreCase)).Take(_pageSize).ToList();
                _tableDatas = _apiDatas;
                if (_pageDatas != null) table.TableUpdate(_pageDatas, _tableDatas);
            }
            else
            {
                UpdatePageData(_currentPage);
                _tableDatas = _apiDatas;
                if (_pageDatas != null) table.TableUpdate(_pageDatas, _tableDatas);
            }
        }
    }
    
    private void AddContract()
    {
        addContract.Show<AddContractModal>(new Dictionary<string, object?>
        {
            {"AddContractData", _addContractData},
            {"AddSubmitAction", EventCallback.Factory.Create(this, OnAdd)}
        }, true);
    }

    private void OnAdd()
    {
        // 추가 코드
    }
    
    void GotoModuSign()
    {

    }
}