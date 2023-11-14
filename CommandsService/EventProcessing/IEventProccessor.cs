namespace CommandService.EventProccessing;
public interface IEventProccessor
{
    void ProccessEvent(string message);
}