using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using YubikStudioCore.Forms;
using YubikStudioCore.Forms.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VendorAudit.WorkItems;
using VendorAudit.BusinessObjects;
using YubikStudioCore.Forms.Fields;
using iText.Layout.Element;

namespace VendorAudit.Forms
{
    public class VendorResult : Form<ExecutionWI>
    {
        public virtual Document VendorReport { get; set; }
        public virtual ToggleField RespHNCZT { get; set; }
        [Unbound]
        public virtual TextField RespHNCZT2 { get; set; }
        public virtual DecimalField RespRating { get; set; }
        [Unbound]
        public virtual TextField RespRating2 { get; set; }
        public virtual MemoField AuditorFinalRemarks { get; set; }
        public virtual DateField AuditDate { get; set; }
        public virtual BoLookupField<AuditType> AuditType { get; set; }
        public virtual BoLookupField<AuditCategory> AuditCategory { get; set; }

        public override FormPart GetLayout()
        {
            var rows = new List<Row>(){
                Row(title: "You are about to send the Final Report to the Vendor contact."),
                Row(Col(AuditType), Col(AuditCategory), Col(AuditDate)),
                Row(Col(RespRating2), Col(RespHNCZT2)),
                Row(Prop(nameof(VendorReport))),
                Row(Col(AuditorFinalRemarks))
            };
            
            return Flat(rows.ToArray());
        }
        public override void ConfigureFields()
        {
            base.ConfigureFields();
            AuditType.ReadOnly = true;
            AuditCategory.ReadOnly = true;
            AuditDate.ReadOnly = true;
            RespHNCZT2.ReadOnly = true;
            RespRating2.ReadOnly = true;
            AuditorFinalRemarks.Rows = 4;

        }
        public override void OnLoad()
        {
            base.OnLoad();
            RespHNCZT2.Value = RespHNCZT.Value ? "Yes" : "No";
            var a = (int)RespRating.Value;
            RespRating2.Value = a + "/100";

        }
    }

}
