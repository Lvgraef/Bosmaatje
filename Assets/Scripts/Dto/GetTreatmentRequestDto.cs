using System;

namespace Dto
{
    public class GetTreatmentRequestDto
    {
        public Guid treatmentId;
        public string treatmentName;
        public string doctorName;
        public string imageUri;
        public string videoUri;
        public DateTime? date;
        public int order;
        public string stickerId;
        public string[] description;
        public bool isCompleted;
    }
}