//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SewingCompany
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Width { get; set; }
        public Nullable<decimal> Height { get; set; }
        public Nullable<int> IdUnitWidth { get; set; }
        public Nullable<int> IdUnitHeight { get; set; }

        public string DisplayName
        {
            get { return Name + " (���. " + Id + ")"; }
        }
    
        public virtual Unit Unit { get; set; }
        public virtual Unit Unit1 { get; set; }
    }
}
