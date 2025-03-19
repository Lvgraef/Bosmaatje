using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Dto
{
    public class GetTreatmentRequestDto
    {
        public string treatmentId;
        public string treatmentName;
        public string treatmentUrl;
        public DateTime treatmentDate;
        public int treatmentOrder;
        public string DoctorName;
        public bool isStickerOpened;
        public int stickerId;
    }
}
