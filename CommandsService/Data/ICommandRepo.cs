public interface ICommandRepo
{
    bool SaveChanges();

    // Platforms related
    IEnumerable<Platform> GetAllPlatforms();

    void CreatePlatform(Platform platform);

    bool PlatformExists(int platformId);

    // Commands related
    IEnumerable<Command> GetCommandsForPlatform(int platformId);

    Command GetCommand(int platformId, int commandId);

    void CreateCommand(int platformId, Command command);

}