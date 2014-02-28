using DADataManager;
using DADataManager.ExportModels;
using System.Collections.Generic;


namespace DataExport.Core.ProfileManagement
{
    static class ProfilesManager
    {
        public static int UserId { get; set; }
        public static List<Profile> Profiles { get; private set; }
        public static Profile CurrentProfile { get; private set; }
        public static List<SimpleFormulaModel> TimeSliceFormulas { get; private set; }
        public static List<SimpleFormulaModel> SnapShootFormulas { get; private set; } 

        public delegate void CurrentProfileChanged(Profile profile);
        public static event CurrentProfileChanged RaiseCurrentProfileChanged;


        static ProfilesManager()
        {
            TimeSliceFormulas = new List<SimpleFormulaModel>();
            SnapShootFormulas = new List<SimpleFormulaModel>();
        }

        public static void LoadProfiles()
        {
            Profiles = new List<Profile>();
            var profileModels = DataExportClientDataManager.GetProfiles(UserId);

            foreach (var profileModel in profileModels)
            {
                var profile = new Profile(profileModel);

                Profiles.Add(profile);
            }
        }

        public static void SetCurrentProfile(string name)
        {
            CurrentProfile = new Profile(Profiles.Find(a => a.Parameters.ProfileName == name).Parameters);

            if (RaiseCurrentProfileChanged != null)
                RaiseCurrentProfileChanged(CurrentProfile);
        }

        public static bool CreateNewProfile(string name)
        {
            if (!Profiles.Exists(a => a.Parameters.ProfileName == name))
            {
                var newProfileModel = new ProfileModel
                    {
                        ProfileName = name,
                        EnableLinkExport = false,
                        EnableScheduleJob = false
                    };
                DataExportClientDataManager.AddNewProfile(newProfileModel, UserId);

                return true;
            }
            return false;
        }

        public static void EditCurrentProfile(ProfileModel newProfileModel)
        {
            DataExportClientDataManager.EditProfile(CurrentProfile.Parameters.ProfileId, newProfileModel);
            LoadProfiles();
            SetCurrentProfile(newProfileModel.ProfileName);
        }

        public static bool RenameCurrentProfile(string name)
        {
            if (!Profiles.Exists(a => a.Parameters.ProfileName == name) && CurrentProfile.Parameters.ProfileName != name)
            {
                var newProfileModel = new ProfileModel
                {
                    ProfileName = name,
                    EnableLinkExport = CurrentProfile.Parameters.EnableLinkExport,
                    EnableScheduleJob = CurrentProfile.Parameters.EnableScheduleJob,
                    SheduleJobs = CurrentProfile.Parameters.SheduleJobs ?? new List<SheduleJobModel>()
                };
                DataExportClientDataManager.EditProfile(CurrentProfile.Parameters.ProfileId, newProfileModel);

                return true;
            }
            return false;
        }

        public static void DeleteCurrentProfile()
        {
            DataExportClientDataManager.DeleteProfile(CurrentProfile.Parameters.ProfileId);
            CurrentProfile = null;
        }

        public static void UpdateTimeSliceFormulas(List<SimpleFormulaModel> formulaList)
        {
            TimeSliceFormulas = new List<SimpleFormulaModel>();
            TimeSliceFormulas = formulaList;
        }

        public static void UpdateSnapShotFormulas(List<SimpleFormulaModel> formulaList)
        {
            SnapShootFormulas = new List<SimpleFormulaModel>();
            SnapShootFormulas = formulaList;
        }

        public static void ClearCurrnetFormulas()
        {
            if (TimeSliceFormulas != null) TimeSliceFormulas.Clear();
            if (SnapShootFormulas != null) SnapShootFormulas.Clear();
        }

        public static void ClearAll()
        {
            Profiles = new List<Profile>();
            CurrentProfile = null;
            TimeSliceFormulas = new List<SimpleFormulaModel>();
            SnapShootFormulas = new List<SimpleFormulaModel>();
        }
    }
}
