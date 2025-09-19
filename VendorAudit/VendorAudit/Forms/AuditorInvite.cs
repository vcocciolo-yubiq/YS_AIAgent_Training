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

namespace VendorAudit.Forms
{
    public class FrmAuditorInvite : Form<ExecutionWI>
    {
        [Unbound]
        public virtual TextField AuditorName { get; set; }
        [Unbound]
        public virtual TextField AuditorType { get; set; }
        [Unbound]
        public virtual TextField AuditorEmail { get; set; }

        public override FormPart GetLayout()
        {
            var r1 = Row(title: "You are sending an invite to the Auditor. A notification  email is going to be sent to the Auditor contact.");
            var r2 = Row(Col(AuditorName), Col(AuditorEmail), Col(AuditorType));
            return Flat(r1, r2);
        }
        public override void OnLoad()
        {
            base.OnLoad();
            AuditorName.Value = Context.Item.AssignedAuditor.FullName;
            AuditorEmail.Value = Context.Item.AssignedAuditor.Email;
            AuditorType.Value = Context.Item.AssignedAuditor.Profile["Position"];
        }
        public override void ConfigureFields()
        {
            base.ConfigureFields();
            AuditorName.ReadOnly = true;
            AuditorEmail.ReadOnly = true;
            AuditorType.ReadOnly = true;
        }
    }
}
