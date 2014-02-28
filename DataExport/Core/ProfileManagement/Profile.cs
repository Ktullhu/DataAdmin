using System.Collections.Generic;
using System.Linq;
using DADataManager.ExportModels;
using DADataManager;

namespace DataExport.Core.ProfileManagement
{
    public class Profile
    {
        public ProfileModel Parameters;
        public List<QueryModel> Queries { get; private set; }
        public QueryModel CurrentQuery;

        public Profile()
        {
            Parameters = new ProfileModel();
            Queries = new List<QueryModel>();
        }

        public Profile(ProfileModel parameters)
        {
            Parameters = parameters;
            Queries = DataExportClientDataManager.GetQueriesForProfile(Parameters.ProfileId);
        }

        public bool AddQuery(QueryModel newQuery)
        {
            if (!Queries.Exists(query => query.QueryName == newQuery.QueryName))
            {
                DataExportClientDataManager.AddQueryToProfile(Parameters.ProfileId, newQuery);
                Queries = DataExportClientDataManager.GetQueriesForProfile(Parameters.ProfileId);
                return true;
            }
            return false;
        }

        public void DeleteQuery(string name)
        {
            var queryId = Queries.Find(query => query.QueryName == name).QueryId;

            DataExportClientDataManager.DeleteQueryFromProfie(Parameters.ProfileId, queryId);
        }

        public QueryModel GetQueryData(string name)
        {
            return Queries.Find(query => query.QueryName == name);
        }

        public List<SheduleJobModel> GetSheduleTimes()
        {
            return Parameters.SheduleJobs.ToList();
        }

        public void EditCurrentQuery(QueryModel newQuery)
        {
            DataExportClientDataManager.EditQuery(CurrentQuery.QueryId, newQuery);
            Queries = DataExportClientDataManager.GetQueriesForProfile(Parameters.ProfileId);
            SetCurrentQuery(newQuery.QueryName);
        }

        public void SetCurrentQuery(string name)
        {
            CurrentQuery = Queries.Find(query => query.QueryName == name);
        }
    }
}
