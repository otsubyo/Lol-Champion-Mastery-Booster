namespace Lol_Champion_Mastery_Booster.Models
{
    public class Profile
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string RiotName { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public User User { get; set; }
    }

}
