using ebrain.admin.bc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebrain.admin.bc.Models
{
    public class MaterialBranch : HistoricalEntity
    {
        public Guid MaterialBranchId { get; set; }
        public Guid? MaterialId { get; set; }
        public Guid? BranchId { get; set; }
    }
}