using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Dto;

namespace Assets.Scripts.Global
{
    public static class UserDataSingleton
    {
        private static string _treatmentPlanName;
        private static string[] _treatmantIds;

        public static void SetTreatmentPlanName(string treatmentPlanName)
        {
            _treatmentPlanName = treatmentPlanName;
        }

        public static void SetTreatmentIds(string[] treatmentIds)
        {
            _treatmantIds = treatmentIds;
        }

        public static string[] GetTreatmentIds()
        {
            return _treatmantIds;
        }

        public static string GetTreatmentPlanName()
        {
            return _treatmentPlanName;
        }



    }
}
