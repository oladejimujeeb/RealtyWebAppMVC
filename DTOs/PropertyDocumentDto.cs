namespace RealtyWebApp.DTOs
{
    public class PropertyDocumentDto
    {
        public string DocumentPath { get; set; }
        public string PropertyRegNo { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public byte[] Data { get; set; }
    }
    
}