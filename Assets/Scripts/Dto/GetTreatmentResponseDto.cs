using System;
using JetBrains.Annotations;

namespace Dto
{
    public class GetTreatmentResponseDto
    {
        public Guid treatmentId;
        public string treatmentName;
        public string doctorName;
        public string imagePath;
        [CanBeNull] public string videoPath;
        public DateTime? date;
        public int order;
        public string stickerId;
        public string[] description;
    }
}