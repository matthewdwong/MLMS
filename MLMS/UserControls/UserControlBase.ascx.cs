using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MLMS.UserControls
{
    [ParseChildren(true)]
    public partial class UserControlBase : UserControl
    {
        #region Properties

        private string _controlTitle;
        public string ControlTitle
        {
            get { return _controlTitle; }
            set { _controlTitle = value; }
        }

        private ITemplate _templateItems;     
        [TemplateInstance(TemplateInstance.Single)]
        [TemplateContainer(typeof(ItemContainer), BindingDirection.TwoWay)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ITemplate TemplateItems
        {
            get
            {
                return _templateItems;
            }
            set
            {
                _templateItems = value;
            }
        }

        #endregion

        [ParseChildren(true)]
        public class ItemContainer : Control, INamingContainer
        {

        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void OnDataBinding(EventArgs e)
        {
            EnsureChildControls();
            base.OnDataBinding(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_templateItems != null)
            {
                var container = new ItemContainer();
                container.ID = "itemContainer";
                _templateItems.InstantiateIn(container);
                phBody.Controls.Add(container);
            }
        }

        protected void lblHeader_Load(object sender, EventArgs e)
        {
            lblHeader.Text = _controlTitle;
        }

        public void AddPlaceHolderControl(Control control)
        {
            phBody.Controls.Add(control);
        }

        public void SetCssForBody(string cssName)
        {
            divUCBody.Attributes["class"] = cssName;
        }

        public void SetMinHeightForBody(double height, UnitType heightType)
        {
            Unit heightUnit = new Unit(height, heightType);
            string heightString = heightUnit.ToString();
            divUCBody.Attributes["style"] = "min-height:"+heightString+";";
        }

        public void SetMinWidthForBody(double width, UnitType widthType)
        {
            Unit widthUnit = new Unit(width, widthType);
            string widthString = widthUnit.ToString();
            divUCBody.Attributes["style"] = "min-width:" + widthString + ";";
        }

        public override Control FindControl(string controlName)
        {
            var itemContainer = phBody.FindControl("itemContainer");
            //foreach (Control control in itemContainer.Controls)
            //{
            //    var foundControl = control.FindControl(controlName);

            //    if (foundControl != null)
            //        return foundControl;
            //}
            return itemContainer.FindControl(controlName);
        }
   }
}