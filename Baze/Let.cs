//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Baze
{
    using System;
    using System.Collections.Generic;
    
    public partial class Let
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Let()
        {
            this.Karta = new HashSet<Karta>();
        }
    
        public string LID { get; set; }
        public string AID_FK { get; set; }
        public string PID_FK { get; set; }
        public string JMBG_FK { get; set; }
    
        public virtual Avion Avion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Karta> Karta { get; set; }
        public virtual Pista Pista { get; set; }
        public virtual Radnik Radnik { get; set; }
    }
}
