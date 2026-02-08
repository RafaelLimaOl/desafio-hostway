namespace DesafioHostway.Models.Request;

public class UserAccountRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public bool HaveDeficiency { get; set; }
    public bool IsActive { get; set; }
}
