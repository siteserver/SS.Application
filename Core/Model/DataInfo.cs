using System;

namespace SS.Application.Core.Model
{
    public class DataInfo
    {
        public int Id { get; set; }

        public int SiteId { get; set; }

        public DateTime AddDate { get; set; }

        public string QueryCode { get; set; }

        public int DepartmentId { get; set; }

        public bool IsCompleted { get; set; }

        public string State { get; set; }

        public string DenyReason { get; set; }

        public string RedoComment { get; set; }

        public string ReplyContent { get; set; }

        public bool IsReplyFiles { get; set; }

        public DateTime ReplyDate { get; set; }

        public bool IsOrganization { get; set; }

        public string CivicName { get; set; }

        public string CivicOrganization { get; set; }

        public string CivicCardType { get; set; }

        public string CivicCardNo { get; set; }

        public string CivicPhone { get; set; }

        public string CivicPostCode { get; set; }

        public string CivicAddress { get; set; }

        public string CivicEmail { get; set; }

        public string CivicFax { get; set; }

        public string OrgName { get; set; }

        public string OrgUnitCode { get; set; }

        public string OrgLegalPerson { get; set; }

        public string OrgLinkName { get; set; }

        public string OrgPhone { get; set; }

        public string OrgPostCode { get; set; }

        public string OrgAddress { get; set; }

        public string OrgEmail { get; set; }

        public string OrgFax { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Purpose { get; set; }

        public bool IsApplyFree { get; set; }

        public string ProvideType { get; set; }

        public string ObtainType { get; set; }

        public string DepartmentName { get; set; }
    }
}
