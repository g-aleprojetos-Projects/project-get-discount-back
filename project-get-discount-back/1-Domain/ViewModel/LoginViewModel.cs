namespace project_get_discount_back.ViewModel
{
    public class LoginViewModel
    {
        public Boolean? Auth { get; set; }
        public string? AccessToken { get; set; }
        public string? Mensagem { get; set; }
        public string? RefreshToken { get; set; }
        public string? Device { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}


