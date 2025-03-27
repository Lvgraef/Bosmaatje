using JetBrains.Annotations;

namespace Dto
{
    public class PutConfigurationRequestDto
    {
        [CanBeNull] public string primaryDoctorName { get; set; }
        [CanBeNull] public string treatmentPlanName { get; set; }
        [CanBeNull] public string characterId { get; set; }
    }
}