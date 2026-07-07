using KanbanBoard.Application.DTOs.Auth;
using KanbanBoard.Presentation.Services;
using MudBlazor;

namespace KanbanBoard.Presentation.Components.Pages.Auth;

public partial class Register
{
    private RegisterDto _registerModel = new();

    private async System.Threading.Tasks.Task HandleRegister()
    {
        try
        {
            var response = await AuthApiService.RegisterAsync(_registerModel);
            if (response != null)
            {
                var authProvider = (CustomAuthenticationStateProvider)AuthProvider;
                await authProvider.NotifyUserAuthentication(response.Token);
                Snackbar.Add("Registration successful!", Severity.Success);
                Navigation.NavigateTo("/workspaces");
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }
}