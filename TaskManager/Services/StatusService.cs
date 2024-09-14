using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Dtos;
using TaskManager.Models;
using TaskManager.Repository;

namespace TaskManager.Services
{
    public class StatusService : ICommonService<StatusDto, InsertStatusDto, UpdateStatusDto, int>
    {
        private IRepository<Status> _repository;
        private IMapper _mapper;
        public List<String> Errors { get;}
        public StatusService([FromKeyedServices("Status")] IRepository<Status> repository,IMapper mapper ) { 
        
        _repository = repository;
         _mapper = mapper;
            Errors = new List<String>();
        }

        public async Task<StatusDto> Add(InsertStatusDto Insertitem)
        {

            if (!_repository.GetByFilter(s => s.StatusName == Insertitem.StatusName).IsNullOrEmpty())
            {
                Errors.Add("Ya existe un estatus con ese nombre");
                return null;
            } 
            Status item =  _mapper.Map<Status>(Insertitem);
            await _repository.Add(item);
            await _repository.Save();
            return _mapper.Map<StatusDto>(item);
            
        }

        public async  Task<StatusDto> Delete(int id)
        {
            Status status = await _repository.GetById(id);
            if (status == null)
            {

                Errors.Add("El Id es Invalido");
                return null;


            }
            var Dto = _mapper.Map<StatusDto>(status);
            _repository.Delete(status);
            await _repository.Save();
            return Dto;
        }

        public  async Task<IEnumerable<StatusDto>> Get()
        {
            var tasksList = await _repository.Get();
           return tasksList.Select(x => _mapper.Map<StatusDto>(x));
        
        }

        
        public IEnumerable<StatusDto> GetByFilter(int filter)
        {
            var Status =  _repository.GetByFilter(x => x.StatusId == filter);
            if (Status == null) return null;
            return Status.Select(x => _mapper.Map<StatusDto>(x));
        }

        public async Task<StatusDto> Update(UpdateStatusDto updatedItem, int id)
        {
            var status = await _repository.GetById(id);
            if (status == null) {

                Errors.Add("El Id es Invalido");
                return null;
                    
                    
                    }
            if (!_repository.GetByFilter(s => s.StatusName == updatedItem.StatusName).IsNullOrEmpty())
            {
                Errors.Add("Ya existe un estatus con ese nombre");
                return null;
            }
            status = _mapper.Map<UpdateStatusDto,Status>(updatedItem, status);       
            _repository.Update(status);
            await _repository.Save();
            return _mapper.Map<StatusDto>(status);
        }
    }
}
