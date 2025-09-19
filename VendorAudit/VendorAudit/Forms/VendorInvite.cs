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
using iText.IO.Font;

namespace VendorAudit.Forms
{
    public class VendorInvite : Form<ExecutionWI>
    {
        [Unbound]
        public virtual TextField VendorName { get; set; }
        [Unbound]
        public virtual TextField VendorEmail { get; set; }

        public override FormPart GetLayout()
        {
            var r1 = Row(title:"You are sending an invite to the vendor. A notification  email is going to be sent to the vendor contact.");
            var r2 = Row(Col(VendorName), Col(VendorEmail));
            var c1 = Flat(r1, r2);
            return c1;
        }
        public override void OnLoad()
        {
            base.OnLoad();
            VendorName.Value = Context.Item.Vendor.Name;
            VendorEmail.Value = Context.Item.Vendor.ContactUser.Email;


        }
        public override void ConfigureFields()
        {
            base.ConfigureFields();
            VendorName.ReadOnly = true;
            VendorEmail.ReadOnly = true;
        }
    }

}
