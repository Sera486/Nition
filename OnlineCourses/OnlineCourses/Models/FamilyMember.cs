namespace OnlineCourses.Models.ManageViewModels
{
    public class FamilyMember
    {
        public int ID { get; set; }
        public ApplicationUser User { get; set; }
        public ApplicationUser Member { get; set; }
    }
}
