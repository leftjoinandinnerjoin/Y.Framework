using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using Y.Framework.EntityFramework.Core.DataEncryption.Configuration;

namespace Y.Framework.EntityFramework.Core.DataEncryption;

public static class DbContextEncryptionBuildExtensions
{
    public static void UseEncryption(this ModelBuilder modelBuilder, Action<EncryptionConfigurationOptions> action)
    {
        var options = new EncryptionConfigurationOptions();
        action.Invoke(options);
        var aesProvider = new AesProvider(Encoding.UTF8.GetBytes(options.Key), Encoding.UTF8.GetBytes(options.Iv));
        modelBuilder.UseEncryption(aesProvider);
    }
}