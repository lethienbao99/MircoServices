using PlatformService.Dtos;

public interface ICommandDataClient
{
    Task SendPlatformToCommand(PlatformReadDto plat);
}