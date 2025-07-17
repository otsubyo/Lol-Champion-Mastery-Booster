namespace Lol_Champion_Mastery_Booster.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public ICollection<Profile> Profiles { get; set; }
    }

}
