using System;

namespace Dto
{
    public class GetTreatmentRequestDto
    {
        public Guid treatmentId;
        public string treatmentName;
        public string doctorName;
        public string imagePath;
        public string videoPath;
        public DateTime? date;
        public int order;
        public string stickerId;
        public string[] description;
        public bool isCompleted;
    }
}