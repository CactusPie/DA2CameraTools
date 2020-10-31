namespace DragonAge2CameraTools.GameManagement.FunctionHooking.Interfaces
{
    public interface IGameFunctionHookService
    {
        void EnableAllHooks();
        void EnableMenuOrDialogueEnteredHook();
        void EnableMenuOrDialogueExitedHook();
        void EnableLoadingScreenEnteredHook();
        void EnableLoadingScreenExitedHook();
        void DisableAllHooks();
        void DisableMenuOrDialogueEnteredHook();
        void DisableMenuOrDialogueExitedHook();
        void DisableLoadingScreenEnteredHook();
        void DisableLoadingScreenExitedHook();
    }
}