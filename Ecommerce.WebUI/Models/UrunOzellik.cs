//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ecommerce.WebUI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UrunOzellik
    {
        public int UrunID { get; set; }
        public int OzellikTipID { get; set; }
        public int OzellikDegerID { get; set; }
    
        public virtual OzellikDeger OzellikDeger { get; set; }
        public virtual OzellikTip OzellikTip { get; set; }
        public virtual Urun Urun { get; set; }
    }
}