using AutoMapper;
using BAL.Models;
using BAL.Services.Interfaces;
using DAL.Infrastructure;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Implements
{
    public class ClassStatusService: IClassStatusService
    {
        private IClassStatusRepository _classStatusRepository;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public ClassStatusService(IClassStatusRepository classStatusRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _classStatusRepository = classStatusRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<ClassStatusViewModel> GetAll()
        {
            return _mapper.Map<List<ClassStatusViewModel>>(_classStatusRepository.GetAll());
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void SaveAsync()
        {
            _unitOfWork.commitAsync();
        }
    }
}
