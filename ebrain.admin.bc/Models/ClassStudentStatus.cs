using ebrain.admin.bc.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebrain.admin.bc.Models
{
    public class ClassStudentStatus : HistoricalEntity
    {
        public long ClassStudentStatusId { get; set; }
        [MaxLength(256)]
        public string ClassStudentStatusCode { get; set; }
        [MaxLength(256)]
        public string ClassStudentStatusName { get; set; }
    }
}