using System;
using Datory;

namespace SS.Application.Core.Model
{
    [Table("ss_application_log")]
    public class LogInfo : Entity
    {
        [TableColumn]
        public int SiteId { get; set; }

        [TableColumn]
        public int DataId { get; set; }

        [TableColumn]
        public int UserId { get; set; }

        [TableColumn]
        public DateTime? AddDate { get; set; }

        [TableColumn]
        public string Summary { get; set; }

        public string UserName { get; set; }
    }
}
