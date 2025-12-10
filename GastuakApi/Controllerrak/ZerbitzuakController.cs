using JatetxeaApi.Modeloak;
using JatetxeaApi.DTOak;
using JatetxeaApi.Repositorioak;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace JatetxeaApi.Controllerrak
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZerbitzuakController : ControllerBase
    {
        private readonly ZerbitzuakRepository _repo;

        public ZerbitzuakController(ZerbitzuakRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var lista = _repo.GetAll().Select(z => new ZerbitzuakDto
            {
                Id = z.Id,
                LangileId = z.LangileId,
                MahaiaId = z.MahaiaId,
                EskaeraData = z.EskaeraData,
                Egoera = z.Egoera,
                Guztira = z.Guztira
            });

            return Ok(lista);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var z = _repo.Get(id);
            if (z == null) return NotFound(new { mezua = "Ez da aurkitu" });

            return Ok(new ZerbitzuakDto
            {
                Id = z.Id,
                LangileId = z.LangileId,
                MahaiaId = z.MahaiaId,
                EskaeraData = z.EskaeraData,
                Egoera = z.Egoera,
                Guztira = z.Guztira
            });
        }

        [HttpPost]
        public IActionResult Sortu([FromBody] ZerbitzuakSortuDto dto)
        {
            var z = new Zerbitzuak(dto.LangileId, dto.MahaiaId, dto.EskaeraData, dto.Egoera, dto.Guztira);
            _repo.Add(z);
            return Ok(new { mezua = "Zerbitzuak sortuta", id = z.Id });
        }

        [HttpPut("{id}")]
        public IActionResult Eguneratu(int id, [FromBody] ZerbitzuakSortuDto dto)
        {
            var z = _repo.Get(id);
            if (z == null) return NotFound(new { mezua = "Ez da aurkitu" });

            z.LangileId = dto.LangileId;
            z.MahaiaId = dto.MahaiaId;
            z.EskaeraData = dto.EskaeraData;
            z.Egoera = dto.Egoera;
            z.Guztira = dto.Guztira;

            _repo.Update(z);
            return Ok(new { mezua = "Eguneratuta" });
        }

        [HttpDelete("{id}")]
        public IActionResult Ezabatu(int id)
        {
            var z = _repo.Get(id);
            if (z == null) return NotFound(new { mezua = "Ez da aurkitu" });

            _repo.Delete(z);
            return Ok(new { mezua = "Ezabatuta" });
        }
    }
}
