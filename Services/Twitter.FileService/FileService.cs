using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Twitter.Entities.Base;
using Twitter.FileService.Models;
using Twitter.Repository;

namespace Twitter.FileService;

public class FileService : IFileService
{
    private readonly IRepository<TwitterFile> _repository;
    private readonly IMapper _mapper;

    public FileService(IRepository<TwitterFile> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<TwitterFileModel>> GetFiles()
    {
        var files = _repository.GetAll();
        var result = (await files.ToListAsync()).Select(x => _mapper.Map<TwitterFileModel>(x));
        return result;
    }

    public Task<TwitterFileModel> GetFileById(Guid id)
    {
        var file = _repository.GetById(id);
        return Task.FromResult(_mapper.Map<TwitterFileModel>(file));
    }

    public Task DeleteFile(Guid id)
    {
        _repository.Delete(_repository.GetById(id));
        return Task.CompletedTask;
    }

    public Task<TwitterFileModel> AddFile(TwitterFileModelRequest requestModel)
    {
        var model = _mapper.Map<TwitterFile>(requestModel);
        return Task.FromResult(_mapper.Map<TwitterFileModel>(_repository.Save(model)));
    }

    public Task<TwitterFileModel> UpdateFile(Guid id, TwitterFileModelRequest requestModel)
    {
        var model = _repository.GetById(id);
        var file = _mapper.Map(requestModel, model);
       // var file = _mapper.Map<TwitterFile>(requestModel);
        return Task.FromResult(_mapper.Map<TwitterFileModel>(_repository.Save(file)));
    }
}