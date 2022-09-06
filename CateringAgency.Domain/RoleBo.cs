using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringAgency.Domain
{
    public class RoleBo
    {
        public int RoleId { get; set; }
        [DataType(DataType.Text)]
        public string RoleName { get; set; }
    }
}
