using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    [MetadataType(typeof(UserModel))]
    public partial class SplitwiseUser : ISpliteWiseUser
    {
        [DisplayName("Confirm Password")]
        [Required]
        [Compare("UserPassword", ErrorMessage="Password doesn't match")]
        public string ConfirmPassword { get; set; }
    }
    class UserModel
    {
        [DisplayName("User Name")]
        [Required]
        public string UserName { get; set; }
        [DisplayName("User Password")]
        [Required]
        public string UserPassword { get; set; }
        [DisplayName("User Role")]
        [Required]
        public Nullable<int> UserRole { get; set; }
    
    }
}
