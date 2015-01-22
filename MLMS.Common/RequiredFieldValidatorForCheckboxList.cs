using System.Web.UI;
using System.Web.UI.WebControls;

namespace MLMS.Common
{
    public class RequiredFieldValidatorForCheckBoxLists : BaseValidator
    {
        private ListControl _listctrl;

        public RequiredFieldValidatorForCheckBoxLists()
        {
            base.EnableClientScript = false;
        }

        protected override bool ControlPropertiesValid()
        {
            Control ctrl = FindControl(ControlToValidate);

            if (ctrl != null)
            {
                _listctrl = (ListControl)ctrl;
                return (_listctrl != null);
            }
            else
                return false;  // raise exception
        }

        protected override bool EvaluateIsValid()
        {
            return _listctrl.SelectedIndex != -1;
        }
    }
}


