using System;

namespace DragonAge2CameraTools.UserInputHandling.KeyHandlers.Enums
{
    [Flags]
    public enum InputResult
    {
        /// <summary>
        /// Allow parsing the hotkey by any following handlers or other applications
        /// </summary>
        Continue = 0,
        /// <summary>
        /// Block any following handlers from processing this input action
        /// </summary>
        StopProcessingEvents = 2,
        /// <summary>
        /// Stops other applications from handling this input action
        /// </summary>
        HideFromOtherApplications = 4,
    }
}