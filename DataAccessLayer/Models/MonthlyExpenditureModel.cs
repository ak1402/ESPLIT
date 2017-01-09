using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    [MetadataType(typeof(MonthlyExpenditureModel))]
    public partial class MonthlyExpenditure
    {
        
    }
    class MonthlyExpenditureModel
    {
        [Required]
        public string Item { get; set; }
        [Required]
        [DisplayName("Date Bought")]
        public Nullable<System.DateTime> DateBought { get; set; }
        [Required]
        public Nullable<decimal> Price { get; set; }
        [DisplayName("Bought By")]

        public Nullable<int> UserId { get; set; }
        [DisplayName("ID")]
        public int ExpID { get; set; }
        [DisplayName("IsIncluded")]
        public Nullable<bool> SharedWithMe { get; set; }
        
    }
}
