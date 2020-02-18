using DragonAge2CameraTools.GameManagement.Data;

namespace DragonAge2CameraTools.GameManagement.Interfaces
{
    public interface IGameValueService
    {
        /// Gets the X coordinate of the point around which the camera rotates
        float GetXCameraPosition();
        
        /// Gets the Y coordinate of the point around which the camera rotates
        float GetYCameraPosition();
        
        /// Gets the Z coordinate (height) of the point around which the camera rotates
        float GetZCameraPosition();
        
        /// Gets the X,Y coordinates of the point around which the camera rotates
        XYCameraPosition GetXYCameraPosition();
        
        /// <summary>
        /// Gets the X,Y,Z coordinates of the point around which the camera rotates
        /// </summary>
        /// <returns></returns>
        XYZCameraPosition GetXYZCameraPosition();
        
        /// <summary>
        /// Get the horizontal camera angle in radians
        /// </summary>
        float GetHorizontalCameraAngle();
        
        /// <summary>
        /// Get the vertical camera angle in radians
        /// </summary>
        float GetVerticalCameraAngle();
        
        /// <summary>
        /// Gets the value of camera distance, where 1 is the closest zoom
        /// and -1 is the furthest zoom. Disabling unlimited zoom allows
        /// to go below -1 
        /// </summary>
        float GetCameraZoomDistance();
        
        /// <summary>
        /// Gets the value of the loading screen flag (a screen that shows while loading a save or changing areas)
        /// </summary>
        /// <returns>True if the game is currently in loading screen, false otherwise</returns>
        bool IsGameInLoadingScreen();
        
        /// <summary>
        /// Gets the value of the menu screen flag (main menu, inventory, etc.)
        /// </summary>
        /// <returns>True if player is in menu, false otherise</returns>
        bool IsGameInMenuScreen();
        
        /// <summary>
        /// Checks whether or not the player is currently in a dialogue
        /// </summary>
        bool IsGameInDialogue();
        
        /// <summary>
        /// Checks whether or not the game is current in dialogue or in menu
        /// </summary>
        bool IsGameInMenuOrDialogue();
        
        /// <summary>
        /// Changes current camera zoom distance where 1 is the closes zoom the game allows and -1 is the furthest
        /// zoom the game allows. Allows to go below -1 if free camera is enabled
        /// </summary>
        /// <param name="newZoomDistance">New zoom distance to set</param>
        void SetCameraZoomDistance(float newZoomDistance);
        
        /// <summary>
        /// Changes the coordinates of the point around which the camera rotates
        /// </summary>
        /// <param name="x">Camera X position</param>
        /// <param name="y">Camera Y position</param>
        void SetCameraPosition(float x, float y);
        
        /// <summary>
        /// Changes the coordinates of the point around which the camera rotates
        /// </summary>
        /// <param name="x">Camera X position</param>
        /// <param name="y">Camera Y position</param>
        /// <param name="z">Camera Z position</param>
        void SetCameraPosition(float x, float y, float z);
        
        /// <summary>
        /// Changes the X coordinate of the point around which the camera rotates
        /// </summary>
        /// <param name="xCameraPosition">New camera X position</param>
        void SetXCameraPosition(float xCameraPosition);
        
        /// <summary>
        /// Changes the Y coordinate of the point around which the camera rotates
        /// </summary>
        /// <param name="yCameraPosition">New camera Y position</param>
        void SetYCameraPosition(float yCameraPosition);
        
        /// <summary>
        /// Changes the Z coordinate of the point around which the camera rotates
        /// </summary>
        /// <param name="zCameraPosition">New camera Z position</param>
        void SetZCameraPosition(float zCameraPosition);
        
        /// <summary>
        /// Allows to zoom out indefinitely
        /// </summary>
        void EnableUnlimitedZoom();
        
        /// <summary>
        /// Disables zooming out infinitely. 
        /// </summary>
        void DisableUnlimitedZoom();
        
        /// <summary>
        /// Prevents the game from changing the came coordinates, which
        /// means that any changes to the camera position we make will
        /// not get immediately overwritten. Essential for enabling tactical camera
        /// </summary>
        void EnableFreeCamera();
        
        /// <summary>
        /// Turns off free camera
        /// </summary>
        void DisableFreeCamera();
        
        /// <summary>
        /// Disables the correction of camera zoom when there is an object between the camera and
        /// the point around which the camera rotates. Prevents the camera from clipping into the walls,
        /// but it's undesirable when tactical camera is enabled
        /// </summary>
        void EnableCollisionZoomAdjustment();
        
        /// <summary>
        /// Disables automatic camera zoom adjustment
        /// </summary>
        void DisableCollisionZoomAdjustment();
        
        /// <summary>
        /// Disables the camera angle automatic adjustment. This usually happens when a character is
        /// going down or up the stairs (or changing its Z position in any other way). Normally this is good,
        /// but when the tactical camera is enabled it results in undesirable behavior.
        /// </summary>
        void EnableAutoCameraAngleAdjustment();
        
        /// <summary>
        /// Disables automatic camera angle adjustment
        /// </summary>
        void DisableAutoCameraAngleAdjustment();
        
        /// <summary>
        /// Disables game zoom handling. Useful when tactical camera is enabled, as then we
        /// handle zoom actions ourselves
        /// </summary>
        void EnableZoom();
        
        /// <summary>
        /// Re-enables default camera zoom handling
        /// </summary>
        void DisableZoom();
    }
}