namespace PersonalSpendingAnalysis.Models
{
    public class LoginResult
    {
        public string jwt { get; set; }
        public string userId { get; set; }
        public bool success { get; set; }
    }
}