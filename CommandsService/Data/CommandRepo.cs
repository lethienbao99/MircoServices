
public class CommandRepo : ICommandRepo
{
    private readonly AppDbContext _appDbContext;
    public CommandRepo(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;  
    }
    public void CreateCommand(int platformId, Command command)
    {
        if(command == null)
            throw new ArgumentNullException(nameof(command));

        command.PlatformId = platformId;
        _appDbContext.Commands.Add(command);
    }

    public void CreatePlatform(Platform platform)
    {
        if(platform == null)
            throw new ArgumentNullException(nameof(platform));

        _appDbContext.Platforms.Add(platform);
    }

    public bool ExternalPlatformExist(int externalPlatformId)
    {
        return _appDbContext.Platforms.Any(s => s.ExternalID == externalPlatformId);
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        return _appDbContext.Platforms.ToList();
    }

    public Command GetCommand(int platformId, int commandId)
    {
        return _appDbContext.Commands
            .Where(s => s.PlatformId == platformId && s.Id == commandId).FirstOrDefault();
    }

    public IEnumerable<Command> GetCommandsForPlatform(int platformId)
    {
        return _appDbContext.Commands.Where(s => s.PlatformId == platformId)
            .OrderBy(s => s.PlatformId);
    }

    public bool PlatformExits(int platformId)
    {
        return _appDbContext.Platforms.Any(s => s.Id == platformId);
    }

    public bool SaveChanges()
    {
        return (_appDbContext.SaveChanges() >= 0);
    }
}