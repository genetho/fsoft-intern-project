using AutoMapper;
using BAL.Models;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;

namespace BAL.Services.Implements
{
    public class MaterialService : IMaterialService
    {
        // Team6
        private IMaterialRepository _materialRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MaterialService(IMaterialRepository materialRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _materialRepository = materialRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public List<MaterialViewModel> GetMaterials(long? id)
        {
            return _mapper.Map<List<MaterialViewModel>>(_materialRepository.GetLessonMaterials(id));
        }

        public void DeleteMaterial(long? id)
        {
            _materialRepository.DeleteMaterial(id);
        }
        // Team6

        // team 6 - viet
        public void UpdateMaterial(MaterialViewModel material)
        {
            //Update Material
            if (material.Id != null)
            {
                Material dbMaterial = _materialRepository.GetById(material.Id.Value);
                if (dbMaterial != null)
                {
                    dbMaterial.Name = material.Name;
                    dbMaterial.HyperLink = material.HyperLink;
                    dbMaterial.Status = material.Status;

                    _materialRepository.Update(dbMaterial);
                }
                else throw new Exception("This material status is inactive");
            }
        }


        // team 6 - viet

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void SaveAsync()
        {
            _unitOfWork.commitAsync();
        }

        public MaterialViewModel GetMaterial(long? id)
        {
            Material dbMaterial = _materialRepository.GetById(id.Value);
            return _mapper.Map<MaterialViewModel>(dbMaterial);
        }
    }
}

