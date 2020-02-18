namespace DragonAge2CameraTools.ViewLogic.Settings.Interfaces
{
    public interface IStringSerializer
    {
        string Serialize(object objectToSerialize);
        T Deserialize<T>(string serializedObject);
    }
}