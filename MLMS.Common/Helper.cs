namespace MLMS.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// This is a helper class with helper methods.
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// Checks if the selected file is of the allowed MimeType.
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static bool IsAllowedMimeType(string contentType)
        {
            string validMimeType = ConfigurationManager.AppSettings["ImageMimeTypes"];
            if (String.IsNullOrEmpty(validMimeType))
                return false;

            string[] mimeTypes = validMimeType.Split(',');
            bool found = false;
            foreach (string type in mimeTypes)
            {
                if (type.Trim() == contentType)
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        /// <summary>
        /// Concatenates the username with URL of the site
        /// </summary>
        /// <param name="userName">The username</param>
        /// <param name="url">The site URL</param>
        /// <returns>a concat of both strings</returns>
        public static string AugmentUsernameWithUrl(string userName, string url)
        {
            return String.Concat(userName, url);
        }

        /// <summary>
        /// Returns the Base URL
        /// </summary>
        /// <param name="context">Http Context Object</param>
        /// <returns>The base URL</returns>
        public static string BaseSiteUrl(HttpContext context)
        {
            ////HttpContext context = HttpContext.Current;
            string baseUrl = context.Request.Url.Scheme + "://" + context.Request.Url.Authority + context.Request.ApplicationPath.TrimEnd('/') + '/';
            return baseUrl;
        }

        /// <summary>
        /// Converts minutes to hours:minutes
        /// </summary>
        /// <param name="minutes">The number of minutes</param>
        /// <returns>a string of hours:minutes</returns>
        public static string ConvertToHourMinuteFormat(int minutes)
        {
            TimeSpan t = TimeSpan.FromMinutes(minutes);

            int calculatedHours = t.Hours;
            int calcuatedmins = t.Minutes;

            StringBuilder time = new StringBuilder();
            
            if (minutes > 59)
            {
                time.Append(calculatedHours);
                time.Append(" ");

                if (calculatedHours > 1)
                {
                    time.Append("hrs");
                }
                else
                {
                    time.Append("hr");
                }

                time.Append(" ");
            }

            if (calcuatedmins > 0)
            {
                time.Append(calcuatedmins);
                time.Append(" ");
                time.Append("mins");
            }

            return time.ToString();
        }

        /// <summary>
        /// Converts Web UI Control SortDirection
        /// to SQL sort direction
        /// </summary>
        /// <param name="sortDirection">Web UI Control Sorting Direction</param>
        /// <returns>Sql Sorting Direction</returns>
        public static string ConvertSortDirectionToSql(SortDirection sortDirection)
        {
            string newSortDirection = String.Empty;

            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    newSortDirection = "ASC";
                    break;

                case SortDirection.Descending:
                    newSortDirection = "DESC";
                    break;
            }

            return newSortDirection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Controls"></param>
        /// <returns></returns>
         
        public static void FindControl<T>(ControlCollection Controls, ref List<T> returnList) where T : class
        {
            //T found = default(T);

            //if (Controls != null && Controls.Count > 0)
            //{
            //    for (int i = 0; i < Controls.Count; i++)
            //    {
            //        if (Controls[i] is T)
            //        {
            //            found = Controls[i] as T;
            //            break;
            //        }
            //        else
            //            found = FindControl<T>(Controls[i].Controls);
            //    }
            //}

            //return found;
            if (Controls != null && Controls.Count > 0)

                foreach (Control controlItem in Controls)
                {

                    if (controlItem is T)

                        returnList.Add(controlItem as T);

                    FindControl<T>(controlItem.Controls, ref returnList);

                }

        }

        #region DropDownList
        public static void BindDropDown(DropDownList list, DataTable table, string textField, string valueField, bool addHeader)
        {
            list.DataTextField = textField;
            list.DataValueField = valueField;
            list.DataSource = table;
            list.DataBind();
            if (addHeader)
                list.Items.Insert(0, new ListItem(" ", "-1"));
        }
        /// <summary>
        /// Bind Collection's Items to the DropDownList's. 
        /// If addHeader is true, first Item in DropDownList will have blank text and a value of -1.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="collection"></param>
        /// <param name="textField"></param>
        /// <param name="valueField"></param>
        /// <param name="addHeader"></param>
        public static void BindDropDown(DropDownList list, ICollection collection, string textField, string valueField, bool addHeader)
        {
            list.DataTextField = textField;
            list.DataValueField = valueField;
            list.DataSource = collection;
            list.DataBind();
            if (addHeader)
                list.Items.Insert(0, new ListItem(" ", "-1"));
        }

        /// <summary>
        /// Bind Collection's Items to the DropDownList's. 
        /// Item with SelectedValue as Value will be marked as selected.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="collection"></param>
        /// <param name="textField"></param>
        /// <param name="valueField"></param>
        /// <param name="addHeader"></param>
        /// <param name="selectedValue"></param>
        public static void BindDropDown(DropDownList list, ICollection collection, string textField, string valueField, bool addHeader, string selectedValue)
        {
            BindDropDown(list, collection, textField, valueField, addHeader);
            SelectItem(list, selectedValue);
        }

        public static void BindDropDown(DropDownList list, Dictionary<int, string> dictionary, bool addHeader, string selectedValue)
        {
            BindDropDown(list, dictionary, addHeader);
            SelectItem(list, selectedValue);
        }
        public static void BindDropDown(DropDownList list, Dictionary<int, string> dictionary, bool addHeader)
        {
            list.Items.Clear();
            if (addHeader)
                list.Items.Add(new ListItem("", "-1"));
            foreach (KeyValuePair<int, string> pair in dictionary)
            {
                list.Items.Add(new ListItem(pair.Value, pair.Key.ToString()));
            }            
        }
        
        public static void SelectItem(DropDownList list, string selectedValue)
        {
            foreach (ListItem item in list.Items)
            {
                if (item.Value == selectedValue)
                {
                    item.Selected = true;
                    break;
                }
            }
        }
        #endregion

        #region Grid
        public static void BindGrid(GridView grid, ICollection collection)
        {
            grid.DataSource = collection;
            grid.DataBind();
        }
        #endregion

        #region DataList
        public static void BindDataList(DataList list, ICollection collection)
        {
            list.DataSource = collection;
            list.DataBind();
        }
        #endregion

        #region ListBox
       
        public static void BindListBox(ListBox box, ICollection collection, string textField, string valueField)
        {
            box.DataTextField = textField;
            box.DataValueField = valueField;
            box.DataSource = collection;
            box.DataBind();
        }

        public static void BindListBox(ListBox box, ICollection collection, string textField, string valueField, string selectedValues)
        {
            BindListBox(box, collection, textField, valueField);
            if (selectedValues != null)
                SelectItems(box, selectedValues);
        }

        public static void SelectItems(ListBox list, string selectedValues)
        {
            string[] selected = selectedValues.Split(',');

            for (int i = 0; i < selected.Length; i++)
            {
                if (list.Items.FindByValue(selected[i].Trim()) != null)
                    list.Items.FindByValue(selected[i].Trim()).Selected = true;
            }
        }
        /// <summary>
        /// TODO for backward compatability so we can build until others get back
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static List<int> GetListboxSelection(ListBox box)
        {
            List<int> selectedList = new List<int>();
            foreach (ListItem item in box.Items)
            {
                if (item.Selected)
                    selectedList.Add(Convert.ToInt32(item.Value));
            }

            return selectedList;
        }
        #endregion
    }
}
