using AutoMapper;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Services;
public class MangaRegisterService : IMangaRegisterService
{
    private readonly IMyAnimeListAPIService _malApiService;
    private readonly IMapper _mapper;
    private readonly IMangasRegisterRepository _mangasRegisterRepository;

    public MangaRegisterService(IMyAnimeListAPIService malApiService, IMapper mapper, IMangasRegisterRepository mangasRegisterRepository)
    {
        _malApiService = malApiService;
        _mapper = mapper;
        _mangasRegisterRepository = mangasRegisterRepository;
    }

    public async Task<MangaRegister?> CreateMangaRegister(int malMangaId)
    {
        var data = await _malApiService.GetMangaByIdAsync(malMangaId);

        var register = _mapper.Map<MangaRegister>(data);

        if (register == null)
        {
            return null;
        }

        await _mangasRegisterRepository.CreateAsync(register);

        return register;
    }
}
