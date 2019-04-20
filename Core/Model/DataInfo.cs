using System;
using Datory;

namespace SS.Application.Core.Model
{
    [Table("ss_application_data")]
    public class DataInfo : Entity
    {
        [TableColumn]
        public int SiteId { get; set; }

        [TableColumn]
        public DateTime? AddDate { get; set; }

        [TableColumn]
        public string QueryCode { get; set; }

        [TableColumn]
        public int DepartmentId { get; set; }

        [TableColumn]
        public bool IsCompleted { get; set; }

        [TableColumn]
        public string State { get; set; }

        [TableColumn(Text = true)]
        public string DenyReason { get; set; }

        [TableColumn(Text = true)]
        public string RedoComment { get; set; }

        [TableColumn(Text = true)]
        public string ReplyContent { get; set; }

        [TableColumn]
        public bool IsReplyFiles { get; set; }

        [TableColumn]
        public DateTime? ReplyDate { get; set; }

        [TableColumn]
        public bool IsOrganization { get; set; }

        [TableColumn]
        public string CivicName { get; set; }

        [TableColumn]
        public string CivicOrganization { get; set; }

        [TableColumn]
        public string CivicCardType { get; set; }

        [TableColumn]
        public string CivicCardNo { get; set; }

        [TableColumn]
        public string CivicPhone { get; set; }

        [TableColumn]
        public string CivicPostCode { get; set; }

        [TableColumn]
        public string CivicAddress { get; set; }

        [TableColumn]
        public string CivicEmail { get; set; }

        [TableColumn]
        public string CivicFax { get; set; }

        [TableColumn]
        public string OrgName { get; set; }

        [TableColumn]
        public string OrgUnitCode { get; set; }

        [TableColumn]
        public string OrgLegalPerson { get; set; }

        [TableColumn]
        public string OrgLinkName { get; set; }

        [TableColumn]
        public string OrgPhone { get; set; }

        [TableColumn]
        public string OrgPostCode { get; set; }

        [TableColumn]
        public string OrgAddress { get; set; }

        [TableColumn]
        public string OrgEmail { get; set; }

        [TableColumn]
        public string OrgFax { get; set; }

        [TableColumn]
        public string Title { get; set; }

        [TableColumn(Text = true)]
        public string Content { get; set; }

        [TableColumn]
        public string Purpose { get; set; }

        [TableColumn]
        public bool IsApplyFree { get; set; }

        [TableColumn]
        public string ProvideType { get; set; }

        [TableColumn]
        public string ObtainType { get; set; }

        [TableColumn]
        public string DepartmentName { get; set; }
    }
}
