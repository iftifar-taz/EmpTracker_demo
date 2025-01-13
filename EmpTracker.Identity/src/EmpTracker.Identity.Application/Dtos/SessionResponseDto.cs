namespace EmpTracker.Identity.Application.Dtos
{
    public class SessionResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
