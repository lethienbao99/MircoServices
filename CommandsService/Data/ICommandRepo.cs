public interface ICommandRepo
{
    bool SaveChanges();

    //Platform
    IEnumerable<Platform> GetAllPlatforms();
    void CreatePlatform(Platform platform);
    bool PlatformExits(int platformId);
    bool ExternalPlatformExist(int externalPlatformId);


    //Command
    IEnumerable<Command> GetCommandsForPlatform(int platformId);
    Command GetCommand(int platformId, int commandId);
    void CreateCommand(int platformId, Command command);    



}