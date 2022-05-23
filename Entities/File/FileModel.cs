using System;

namespace RealtyWebApp.Entities.File
{
    public abstract class FileModel:BaseEntity
    {
        public string DocumentName { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public int UploadedBy { get; set; }
        public string PropertyRegNo { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}