using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;
using YubikStudioCore.Forms;
using YubikStudioCore.Forms.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ENELDAI.WorkItems;
using ENELDAI.BusinessObjects;

namespace ENELDAI.Forms
{
    public class AIRetFieldRow : SubForm<AIRetField>
    {
        public virtual TextField Name { get; set; }
        public virtual TextField Value { get; set; }
        public virtual DecimalField Confidence { get; set; }
        public override void ConfigureFields()
        {
            base.ConfigureFields();
            Name.ReadOnly = true;
            Name.ColumnWidth = "30%";

            Value.ReadOnly = true;
            Name.ColumnWidth = "50%";

            Confidence.ReadOnly = true;
            Confidence.ColumnWidth = "20%";
        }
    }

}
