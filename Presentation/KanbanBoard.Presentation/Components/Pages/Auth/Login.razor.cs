using KanbanBoard.Application.DTOs.Auth;
using KanbanBoard.Presentation.Services;
using MudBlazor;

namespace KanbanBoard.Presentation.Components.Pages.Auth;

public partial class Login
{
    private LogInDto _loginModel = new();

    private async Task HandleLogin()
    {
        try
        {
            var response = await AuthApiService.LoginAsync(_loginModel);
            if (response != null)
            {
                var authProvider = (CustomAuthenticationStateProvider)AuthProvider;
                await authProvider.NotifyUserAuthentication(response.Token);
                Snackbar.Add("Login successful!", Severity.Success);
                Navigation.NavigateTo("/workspaces");
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }
}