namespace SS.Application.Core.Model
{
    public class FileInfo
    {
        public int Id { get; set; }

        public int SiteId { get; set; }

        public int DataId { get; set; }

        public string FileName { get; set; }

        public string FileUrl { get; set; }

        public int Length { get; set; }
    }
}
