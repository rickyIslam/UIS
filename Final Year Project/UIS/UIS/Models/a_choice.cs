//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UIS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class a_choice
    {
        public a_choice()
        {
            this.a_alloted = new HashSet<a_alloted>();
        }
    
        public int id { get; set; }
        public string userRoll { get; set; }
        public int merit_position { get; set; }
        public string status { get; set; }
        public string first_choice { get; set; }
        public string second_choice { get; set; }
        public string third_choice { get; set; }
        public string forth_choice { get; set; }
        public string fifth_choice { get; set; }
        public string sixth_choice { get; set; }
        public string seventh_choice { get; set; }
        public string first_hall { get; set; }
        public string second_hall { get; set; }
        public string third_hall { get; set; }
        public string forth_hall { get; set; }
    
        public virtual ICollection<a_alloted> a_alloted { get; set; }
    }
}