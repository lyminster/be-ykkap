using AutoMapper;
using Database.Models;
using Database.Repositories;
using Database.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModel.Constant;
using ViewModel.ViewModels;

namespace DAL.DataAccessLayer.Master
{
    public class HelperTableDataAccessLayer
    {
        private readonly BusinessModelContext MasterEntities;
        private readonly IMapper Mapper;
        private readonly IHostingEnvironment hostenv;

        public HelperTableDataAccessLayer(BusinessModelContext _MasterEntities, IMapper _mapper, IHostingEnvironment _hostenv)
        {
            Mapper = _mapper;
            hostenv = _hostenv;
            MasterEntities = _MasterEntities;
        }

        public HelperTableRepository _helperRepo;
        public HelperTableRepository helperRepo
        {
            get
            {
                if (_helperRepo == null)
                {
                    _helperRepo = new HelperTableRepository(MasterEntities);
                }
                return _helperRepo;
            }
        }
        #region Method

        public List<HelperTableVM> FilterHelper(HelperTable filter)
        {
            var result = helperRepo.GetAll(filter);
            return Mapper.Map<List<HelperTable>, List<HelperTableVM>>(result);
        }
        public HelperTable GetHelperByCode(string name, string code)
        {
            try
            {
                return MasterEntities.HelperTables.FirstOrDefault(x => x.Name.ToUpper() == name.ToUpper() && x.Code == code);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public HelperTableVM GetHelperTable(string id)
        {
            try
            {
                var result = MasterEntities.HelperTables.FirstOrDefault(x => x.ID.ToUpper() == id.ToUpper());
                return Mapper.Map<HelperTable, HelperTableVM>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public JsonReturn Create(HelperTableVM data)
        {
            JsonReturn jsonReturn = new JsonReturn(false);

            try
            {
                var updateData = Mapper.Map<HelperTableVM, HelperTable>(data);
                updateData.ID = data.ID;
                updateData.CreatedBy = data.CreatedBy;
                updateData.CreatedTime = DateTime.Now;
                updateData.LastModifiedBy = data.LastModifiedBy;
                updateData.LastModifiedTime = DateTime.Now;

                helperRepo.AddNewRequest(updateData);
                helperRepo.Commit();

                jsonReturn.message = "Success";
                jsonReturn.result = true;
                return jsonReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonReturn Edit(HelperTableVM data)
        {
            JsonReturn jsonReturn = new JsonReturn(false);

            try
            {
                HelperTable updateData = helperRepo.GetHelperTable(data.ID);
                updateData.Code = data.Code;
                updateData.Name = data.Name;
                updateData.Value = data.Value;
                updateData.Description = data.Description;
                updateData.LastModifiedBy = data.LastModifiedBy;
                updateData.LastModifiedTime = DateTime.Now;

                helperRepo.Commit();

                jsonReturn.message = "Success";
                jsonReturn.result = true;
                return jsonReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
