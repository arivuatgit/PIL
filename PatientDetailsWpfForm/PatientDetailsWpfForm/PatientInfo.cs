using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDetailsWpfForm
{
   public class PatientInfo
    {
        public long RecordsID { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string SSN { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PState { get; set; }
        public string PCountry { get; set; }
        public string ZIPCode { get; set; }
        public string ArchiveLabel { get; set; }
        public string ContactNumber1 { get; set; }
        public string ContactNumber2 { get; set; }
        public string PEmail { get; set; }
        public string RefDoctor { get; set; }
        public DateTime LastVisit { get; set; }
        public string Diagnosis { get; set; }
        public string FileName { get; set; }
        public string Hospital { get; set; }
        public string ODate { get; set; }
        public string OTime { get; set; }
    }
}
