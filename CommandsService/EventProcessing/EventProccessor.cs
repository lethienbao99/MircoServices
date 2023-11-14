using System.Text.Json;
using AutoMapper;
using CommandService.EventProccessing;

public class EventProccessor: IEventProccessor
{
    private readonly IServiceScopeFactory _serviceScopeFactor;
    private readonly IMapper _mapper;

    public EventProccessor(IServiceScopeFactory serviceScopeFactor, IMapper mapper)
    {
        _serviceScopeFactor = serviceScopeFactor;
        _mapper = mapper;
    }
    public void ProccessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.PlatformPublished:
                addPlatform(message);
                break;
            default:
                break;
        }       
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        Console.WriteLine("--> Determining Event");
        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        switch (eventType.Event)
        {
            case "Platform_Published":
                Console.WriteLine("Platform Published Event Detected");
                return EventType.PlatformPublished;
            default:
                return EventType.Undetermind;

        }
    }

     private void addPlatform(string platformPublishedMessage)
     {
        using(var scope = _serviceScopeFactor.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
            var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

            try
            {
                var plat = _mapper.Map<Platform>(platformPublishedDto);
                if(!repo.ExternalPlatformExist(plat.ExternalID))
                {
                    repo.CreatePlatform(plat);
                    repo.SaveChanges();
                    Console.WriteLine($"--> Platform added...");
                }
                else
                {
                    Console.WriteLine($"--> Platform already exists...");
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"--> Could not add platform to DB {e.Message}");
            }



        }
     }
    

    enum EventType 
    {
        PlatformPublished,
        Undetermind
    }
}