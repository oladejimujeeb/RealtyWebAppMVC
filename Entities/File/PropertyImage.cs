namespace RealtyWebApp.Entities.File
{
    public class PropertyImage:FileModel
    {
        public string DocumentPath { get; set; }
        public Property Property { get; set; }
    }
}