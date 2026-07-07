using KanbanBoard.Presentation.Services;
using MudBlazor;

namespace KanbanBoard.Presentation.Components.Pages.Auth;

public partial class Logout
{
    protected override async Task OnInitializedAsync()
    {
        var authProvider = (CustomAuthenticationStateProvider)AuthProvider;
        await authProvider.NotifyUserLogout();
        Snackbar.Add("Logged out successfully.", Severity.Info);
        Navigation.NavigateTo("/login");
    }
}