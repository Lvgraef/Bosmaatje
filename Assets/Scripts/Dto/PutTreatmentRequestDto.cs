using System;
using JetBrains.Annotations;

namespace Dto
{
    public class PutTreatmentRequestDto
    {
        [CanBeNull] public string doctorName;
        public DateTime? date;
        [CanBeNull] public string stickerId;
    }
}