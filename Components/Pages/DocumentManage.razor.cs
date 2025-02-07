using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using StartupWing_BlazorServer.Components.Datas;
using StartupWing_BlazorServer.Components.Modals;
using StartupWing_BlazorServer.Components.Templates;

namespace StartupWing_BlazorServer.Components.Pages;

public partial class DocumentManage : ComponentBase
{
    List<DocumentData>? _apiDatas = new();
    List<DocumentData>? _pageDatas = new();
    List<DocumentData>? _tableDatas = new();
    
    private int _currentPage = 1;
    private int _pageSize = 10;
    
    DocumentData? _selectDocumentData = new ();
    DocumentData _addDocumentData = new ();
    UserData? _myData = new ();
    
    private Modal addDocument;
    private Modal popup;
    
    private SearchTemplate.DataSort currentSortData = SearchTemplate.DataSort.Descending;
    private TableTemplate<DocumentData> table;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (DataManager.MyData != null)
            {
                _myData = DataManager.MyData;
                await GetMyDocumentDatas(_myData.OrganizationId);
            }
        }
    }
    
    private async Task GetMyDocumentDatas(int? myOrganizationId)
    {
        _apiDatas = await APIService.GetDocumentData_OrganizationID(myOrganizationId);
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
                _pageDatas = _apiDatas?.Where(item => (item.Title ?? "").ToString().Contains(searching) ||
                                                      (item.FileData[0].FileName ?? "").Contains(searching, StringComparison.OrdinalIgnoreCase) ||
                                                      (item.Note ?? "").Contains(searching, StringComparison.OrdinalIgnoreCase)
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

    private void AddDocument()
    {
        addDocument.Show<AddDocumentModal>(new Dictionary<string, object?>
        {
            {"AddDocumentData", _addDocumentData},
            {"AddSubmitAction", EventCallback.Factory.Create(this, OnAdd)}
        }, true);
    }

    private void OnAdd()
    {
        // 서류 추가 코드
    }
}