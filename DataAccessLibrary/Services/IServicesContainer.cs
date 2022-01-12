namespace TaskManager.DataAccess.Services
{
    /// <summary>
    /// Container interface for all services of the business logic of the data layer 
    /// </summary>
    public interface IServicesContainer
    {
        UserService UserServices { get; set; }

        MissionService MissionServices { get; set; }

        Statistics Statistics { get; set; }
    }
}
