using System.Diagnostics;
using DragonAge2CameraTools.GameManagement.Interfaces;

namespace DragonAge2CameraTools.GameManagement.Factories.Interfaces
{
    public interface IAddressFinderFactory
    {
        IAddressFinder CreateAddressFinder(Process process);
    }
}