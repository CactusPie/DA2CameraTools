using System.Diagnostics;
using System.Threading.Tasks;
using DragonAge2CameraTools.GameManagement.Factories.Interfaces;
using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.ProcessMemoryAccess.Interfaces;

namespace DragonAge2CameraTools.GameManagement
{
    public class CodeInjectionReadinessChecker : ICodeInjectionReadinessChecker
    {
        private readonly IProcessFunctionsService _processFunctionsService;
        private readonly IAddressFinderFactory _addressFinderFactory;

        public CodeInjectionReadinessChecker(IProcessFunctionsService processFunctionsService, IAddressFinderFactory addressFinderFactory)
        {
            _processFunctionsService = processFunctionsService;
            _addressFinderFactory = addressFinderFactory;
        }
        
        /// <inheritdoc/>>
        public async Task WaitUntilCodeIsReadyToBeInjected(Process gameProcess)
        {
            IAddressFinder addressFinder = _addressFinderFactory.CreateAddressFinder(gameProcess);
            
            await WaitUntilSaveGameLoaded(gameProcess, addressFinder).ConfigureAwait(false);
            await WaitUntilNotInLoadingScreen(gameProcess, addressFinder).ConfigureAwait(false);
        }

        private async Task WaitUntilSaveGameLoaded(Process gameProcess, IAddressFinder addressFinder)
        {
            int saveGameLoadedFlagAddress = addressFinder.GetSaveGameLoadedFlagAddress();
            while (true)
            {
                int saveGameLoadedFlag = _processFunctionsService.ReadMemoryInt(gameProcess.Handle, saveGameLoadedFlagAddress);
                bool isSaveGameLoaded = (saveGameLoadedFlag & 1) == 1;
                if (isSaveGameLoaded)
                {
                    break;
                }

                await Task.Delay(1000).ConfigureAwait(false);
            }
        }
        
        private async Task WaitUntilNotInLoadingScreen(Process gameProcess, IAddressFinder addressFinder)
        {
            int loadingScreenFlagAddress = addressFinder.GetLoadingScreenFlagAddress();
            while (true)
            {
                int loadingScreenFlag = _processFunctionsService.ReadMemoryInt(gameProcess.Handle, loadingScreenFlagAddress);
                bool isInLoadingScreen = (loadingScreenFlag & 1) == 1;
                if (!isInLoadingScreen)
                {
                    break;
                }

                await Task.Delay(1000).ConfigureAwait(false);
            }
        }
    }
}