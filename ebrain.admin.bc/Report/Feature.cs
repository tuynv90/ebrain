using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebrain.admin.bc.Report
{
    public class Feature
    {
        public Guid ID { get; set; }
        public Guid? GroupID { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }
}
