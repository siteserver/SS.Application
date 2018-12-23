using System;

namespace SS.Application.Core.Model
{
    public class LogInfo
    {
        public int Id { get; set; }

        public int SiteId { get; set; }

        public int DataId { get; set; }

        public int UserId { get; set; }

        public DateTime AddDate { get; set; }

        public string Summary { get; set; }

        // not exists

        public string UserName { get; set; }
    }
}
