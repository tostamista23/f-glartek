
using API.Domain;
using API.Model.Filters;
using API.Model.PagedList;
using API.Database;
using DotNetCore.Results;
using static System.Net.HttpStatusCode;
using Microsoft.EntityFrameworkCore;

namespace API.Application
{
    public interface ICronJobService
    {
        Task<Result> DeleteAsync(long id);
        Task<Result> UpdateAsync(CronJobUpdate dto);
        Task<Result<PagedListResult<IEnumerable<CronJobDTO>>>> ListAsync(BasicFilter filtros);
        Task<Result<long>> CreateAsync(CronJobAdd cron);
        Task<Result<CronJobDTO>> GetAsync(long id);

    }

    public class CronJobService : ICronJobService
    {
        public CronJobService
        (
            IUnitOfWork unitOfWork,
            ICronJobRepository cronJobRepository
        )
        {
            UnitOfWork = unitOfWork;
            CronJobRepository = cronJobRepository;
        }

        private IUnitOfWork UnitOfWork { get; }
        private ICronJobRepository CronJobRepository { get; }

        public async Task<Result<PagedListResult<IEnumerable<CronJobDTO>>>> ListAsync(BasicFilter filtros)
        {
            var validFilter = new BasicFilter(filtros);

            var result = await CronJobRepository.ListAsync(validFilter);

            List<CronJobDTO> listaDTOs = new();

            if (result.Item2 != null)
            {
                foreach (CronJob cron in result.Item2)
                    listaDTOs.Add(new CronJobDTO()
                    {
                        Id = cron.Id,
                        Uri = cron.Uri,
                        HttpMethod = cron.HttpMethod,
                        Body = cron.Body,
                        Schecule = cron.Schecule,
                        TimeZone = cron.TimeZone
                    });
            }

            return new Result<PagedListResult<IEnumerable<CronJobDTO>>>(OK, PagedListHelper.CreatePagedResponse(listaDTOs, validFilter.PageIndex, filtros.PageSize,result.Item1));
        }

        public async Task<Result> UpdateAsync(CronJobUpdate dto)
        {
            CronJob cron = CronJobRepository.Queryable.SingleOrDefault(X => X.Id == dto.Id);

            if (cron == null)
            {
                return new Result<bool>(BadRequest, "Id not found");
            }

            cron.Schecule = dto.Schecule;
            cron.HttpMethod = dto.HttpMethod;
            cron.Uri = dto.Uri;
            cron.TimeZone = dto.TimeZone;
            cron.Body = dto.Body;

            try
            {
                CronJobRepository.Update(cron);
                //Guardar efetivamente na base de dados
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new Result(BadRequest, "Ocorreu um erro ao alterar.");
            }

            return new Result<bool>(OK, true);
        }

        public async Task<Result<long>> CreateAsync(CronJobAdd dto)
        {
            CronJob proj = new CronJob()
            {
                Uri = dto.Uri,
                Body = dto.Body,
                Schecule = dto.Schecule,
                TimeZone = dto.TimeZone,
                HttpMethod = dto.HttpMethod,
            };

            try
            {
                await CronJobRepository.AddAsync(proj);
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new Result<long>(BadRequest, "Ocorreu um erro ao criar");
            }

            return new Result<long>(OK, proj.Id);
        }

        public async Task<Result<CronJobDTO>> GetAsync(long id)
        {
            CronJob cron = await CronJobRepository.Queryable.SingleOrDefaultAsync(X => X.Id == id);

            if (cron == null)
            {
                return new Result<CronJobDTO>(BadRequest, "Id not found");
            }

            return new Result<CronJobDTO>(OK, new CronJobDTO()
            {
                Id = cron.Id,
                Uri = cron.Uri,
                HttpMethod = cron.HttpMethod,
                Body = cron.Body,
                Schecule = cron.Schecule,
                TimeZone = cron.TimeZone
            });
        }

        public async Task<Result> DeleteAsync(long id)
        {
            if (!CronJobRepository.Queryable.Any(X => X.Id == id))
            {
                return new Result<CronJobDTO>(BadRequest, "Id not found");
            }

            try
            {
                await CronJobRepository.DeleteAsync(id);
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new Result(BadRequest, "Ocorreu um erro ao criar");
            }

            return new Result(OK);
        }
    }
}
