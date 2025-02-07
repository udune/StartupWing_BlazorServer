using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace StartupWing_BlazorServer.Components.Templates;

public partial class SearchTemplate : ComponentBase
{
    public enum DataSort
    {
        Descending,
        Ascending
    }
    
    private void OnSearch(KeyboardEventArgs e)
    {
        OnSearchAction?.Invoke(e, searching);
    }

    private void OnButton()
    {
        ButtonAction?.Invoke();
    }

    private void OnChange()
    {
        OnChangeAction?.Invoke(searching);
    }

    private void SortByDescending()
    {
        if (currentSortData.Equals(DataSort.Ascending))
        {
            currentSortData = DataSort.Descending;
            SortByDescendingAction?.Invoke();
        }
    }

    private void SortByAscending()
    {
        if (currentSortData.Equals(DataSort.Descending))
        {
            currentSortData = DataSort.Ascending;
            SortByAscendingAction?.Invoke();
        }
    }
}