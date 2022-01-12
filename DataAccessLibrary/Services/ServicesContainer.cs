using TaskManager.DataAccess.Repositories;

namespace TaskManager.DataAccess.Services
{
    /// <summary>
    /// Container for all services of the business logic of the data layer 
    /// </summary>
    public class ServicesContainer : IServicesContainer
    {
        public UserService UserServices  { get; set; }
        
        public MissionService MissionServices { get; set; }
        
        public Statistics Statistics { get; set ; }

        public ServicesContainer(RepositoriesContainer repositories)
        {
            UserServices = new UserService(repositories.UserRepository);
            MissionServices = new MissionService(repositories.MissionRepository);
            Statistics = new Statistics(repositories);
        }
    }
}
