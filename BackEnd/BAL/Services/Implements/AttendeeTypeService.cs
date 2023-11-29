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
    public class AttendeeTypeService: IAttendeeTypeService
    {
        private IAttendeeTypeRepository _attendeeTypeRepository;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public AttendeeTypeService(IAttendeeTypeRepository attendeeTypeRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _attendeeTypeRepository = attendeeTypeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<AttendeeTypeViewModel> GetAll()
        {
            return _mapper.Map<List<AttendeeTypeViewModel>>(_attendeeTypeRepository.GetAll());
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
