namespace RealtyWebApp.Entities.File
{
    public class PropertyDocument:FileModel
    {
        public byte[] Data { get; set; }
       
        public Property Property { get; set; }
    }
}