namespace Nition.Models
{
    public class FamilyMember
    {
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }
        public string MemberID { get; set; }
        public ApplicationUser Member { get; set; }
    }
}
