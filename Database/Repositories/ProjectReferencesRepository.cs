using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class ProjectReferencesRepository
    {
        private readonly BusinessModelContext _dbContext;

        private readonly IQueryable<ProjectReferences> _q_QueryData;
        public ProjectReferencesRepository(BusinessModelContext expenseManagementContext)
        {
            _dbContext = expenseManagementContext;

            _q_QueryData = _dbContext.ProjectReferences;

        }

        public List<ProjectReferences> GetListProjectReferences()
        {

            try
            {
                return _q_QueryData.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<ProjectReferences> QueryProjectReferencess(Expression<Func<ProjectReferences, bool>> expression, int take = 0, int skip = 0)
        {

            try
            {
                if (take < 1)
                {
                    return _q_QueryData.Where(x => x.RowStatus == 0).Where(expression).ToList();
                }
                else
                {
                    return _q_QueryData.Where(x => x.RowStatus == 0).Where(expression).Skip(skip).Take(take).ToList(); ;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ProjectReferences GetProjectByID(string ID)
        {
            try
            {
                return _q_QueryData.FirstOrDefault(x => x.ID == ID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public bool IsUniqueKeyCodeExist(string name, string detail, string building, 
            string urlImage, string type,string location, string listProductUsed, string projectYear, String idclient)
        {
            return _q_QueryData.Where(x => x.RowStatus == 0).FirstOrDefault(x =>
            x.name == name
            && x.detail == detail
            && x.building == building
            && x.urlImage == urlImage
            && x.type == type
            && x.location == location
            && x.listProductUsed == listProductUsed
            && x.projectYear == projectYear
            ) != null ? true : false;
        }
    }
}
