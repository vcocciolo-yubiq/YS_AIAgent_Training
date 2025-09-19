using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using YubikStudioCore.Forms;
using YubikStudioCore.Forms.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VendorAudit.WorkItems;
using VendorAudit.BusinessObjects;

namespace VendorAudit.Forms
{
    public class ExcelEdit : Form<ExecutionWI>
    {
        public virtual Document ExcelChecklistDL { get; set; }
        public virtual Document ExcelChecklistUL { get; set; }
        public override FormPart GetLayout()
        {
            var r1=Row(title: "Download the checklist in Excel format to be able to edit when you are offline",cssClass: "FirstSeparator");
            var r2 = Row(Col(Prop(nameof(ExcelChecklistDL))));
            var r3=Row(title: "Once you are back online, upload the edited checklist",cssClass: "Separator");
            var r4 = Row(Col(Prop(nameof(ExcelChecklistUL))));
            return Flat(r1,r2,r3,r4);
        }
        public override void ConfigureFields()
        {
            base.ConfigureFields();
            //ExcelChecklist.ReadOnly = false;
        }
    }

}
