namespace MLMS.Objects
{
    public class RequiredField
    {
        #region Properties
        /// <summary>
        /// Gets or sets the required field id
        /// </summary>
        public int RequiredFieldID { get; set; }
        
        /// <summary>
        /// Gets or sets the field name from the db table
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the usercontrol Object name
        /// </summary>
        public string ObjectName { get; set; }
        
        /// <summary>
        /// Gets or sets if the field is required
        /// </summary>
        public bool IsRequired { get; set; }
        
        /// <summary>
        /// Gets or sets if the fields is active
        /// </summary>
        public bool IsActive { get; set; }
             
        #endregion
    }
}
