using System.Security.Cryptography;

namespace Y.Framework.EntityFramework.Core.Entity;

public class SequentialGuidEntity : BaseEntity, IBaseEntity<Guid>
{
    public Guid Id { get; set; } = SequentialGuidTool.GenerateSequentialGuid();
}

class SequentialGuidTool
{
    private static readonly RandomNumberGenerator RandomNumberGenerator = RandomNumberGenerator.Create();
    public static Guid GenerateSequentialGuid()
    {
        var randomBytes = new byte[10];
        RandomNumberGenerator.GetBytes(randomBytes);
        var timestamp = DateTime.UtcNow.Ticks / 10000L;
        var timestampBytes = BitConverter.GetBytes(timestamp);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(timestampBytes);
        }
        var guidBytes = new byte[16];
        Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
        Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
        return new Guid(guidBytes);
    }
}