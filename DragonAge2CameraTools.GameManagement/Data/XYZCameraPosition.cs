namespace DragonAge2CameraTools.GameManagement.Data
{
    public class XYZCameraPosition
    {
        public float XPosition { get; }
        public float YPosition { get; }
        public float ZPosition { get; }

        public XYZCameraPosition(float xPosition, float yPosition, float zPosition)
        {
            XPosition = xPosition;
            YPosition = yPosition;
            ZPosition = zPosition;
        }
    }
}