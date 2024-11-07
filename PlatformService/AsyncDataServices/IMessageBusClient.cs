public interface IMessageBusClient
{
    void PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
}