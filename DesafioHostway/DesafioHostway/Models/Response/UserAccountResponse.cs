namespace DesafioHostway.Models.Response;

public class UserAccountResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public bool HaveDeficiency { get; set; }
    public bool IsActive { get; set; }

}
