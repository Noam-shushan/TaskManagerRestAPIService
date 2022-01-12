using TaskManager.DataAccess.Models;

namespace TaskManager.DataAccess.Repositories
{
    public class RepositoriesContainer
    {
        public IModelRepository<User> UserRepository { get; init; }

        public IModelRepository<Mission> MissionRepository { get; init; }

        public RepositoriesContainer(IModelRepository<User> userRepo, IModelRepository<Mission> missionRepo)
        {
            UserRepository = userRepo;
            MissionRepository = missionRepo;
        }
    }
}
