using PlatformService.Model;

namespace PlatformService.Data 
{
    public interface IPlatFormRepo
    {
        bool SaveChanges();
        IEnumerable<Platform> GetAllFlatforms();
        Platform GetPlatformById(int id);
        void CreatePlatform(Platform plat);

    }
}