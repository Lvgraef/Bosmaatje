using System;

namespace Dto
{
    public class GetConfigurationsRequestDto
    {
        public Guid configurationId;
        public string childName;
        public DateTime childBirthDate;
        public string primaryDoctorName;
        public string characterId;
        public string treatmentPlanName;
    }
}