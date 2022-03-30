namespace AirlineReservationSystem.Core.Models.AdminArea.Users
{
    public class UserRolesVM
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        //all selected roles
        public string[] RoleNames { get; set; }

    }
}
