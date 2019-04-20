﻿using Datory;

namespace SS.Application.Core.Model
{
    [Table("ss_application_department")]
    public class DepartmentInfo : Entity
    {
        [TableColumn]
        public int SiteId { get; set; }

        [TableColumn]
        public string DepartmentName { get; set; }

        [TableColumn(Text = true)]
        public string UserNames { get; set; }

        [TableColumn]
        public int Taxis { get; set; }
    }
}
