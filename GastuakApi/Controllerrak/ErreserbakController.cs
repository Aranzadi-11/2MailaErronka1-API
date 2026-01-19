using JatetxeaApi.Modeloak;
using JatetxeaApi.DTOak;
using JatetxeaApi.Repositorioak;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace JatetxeaApi.Controllerrak
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErreserbakController : ControllerBase
    {
        private readonly ErreserbakRepository _repo;
        private readonly ZerbitzuakRepository _zerbitzuRepo;

        public ErreserbakController(ErreserbakRepository repo, ZerbitzuakRepository zerbitzuRepo)
        {
            _repo = repo;
            _zerbitzuRepo = zerbitzuRepo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var lista = _repo.GetAll().Select(e => new ErreserbakDto
            {
                Id = e.Id,
                MahaiaId = e.MahaiaId,
                Izena = e.Izena,
                Telefonoa = e.Telefonoa,
                ErreserbaData = e.ErreserbaData,
                PertsonaKop = e.PertsonaKop,
                Egoera = e.Egoera,
                Oharrak = e.Oharrak
            });

            return Ok(lista);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var e = _repo.Get(id);
            if (e == null) return NotFound(new { mezua = "Ez da aurkitu" });

            return Ok(new ErreserbakDto
            {
                Id = e.Id,
                MahaiaId = e.MahaiaId,
                Izena = e.Izena,
                Telefonoa = e.Telefonoa,
                ErreserbaData = e.ErreserbaData,
                PertsonaKop = e.PertsonaKop,
                Egoera = e.Egoera,
                Oharrak = e.Oharrak
            });
        }

        [HttpPost]
        public IActionResult Sortu([FromBody] ErreserbakSortuDto dto)
        {
            var e = new Erreserbak(dto.MahaiaId, dto.Izena, dto.Telefonoa, dto.ErreserbaData, dto.PertsonaKop, dto.Egoera, dto.Oharrak);
            _repo.Add(e);
            return Ok(new { mezua = "Erreserba sortuta", id = e.Id });
        }

        [HttpPut("{id}")]
        public IActionResult Eguneratu(int id, [FromBody] ErreserbakSortuDto dto)
        {
            var e = _repo.Get(id);
            if (e == null) return NotFound(new { mezua = "Ez da aurkitu" });

            e.MahaiaId = dto.MahaiaId;
            e.Izena = dto.Izena;
            e.Telefonoa = dto.Telefonoa;
            e.ErreserbaData = dto.ErreserbaData;
            e.PertsonaKop = dto.PertsonaKop;
            e.Egoera = dto.Egoera;
            e.Oharrak = dto.Oharrak;

            _repo.Update(e);
            return Ok(new { mezua = "Eguneratuta" });
        }

        [HttpDelete("{id}")]
        public IActionResult Ezabatu(int id)
        {
            var e = _repo.Get(id);
            if (e == null) return NotFound(new { mezua = "Ez da aurkitu" });

            var lotutakoZerbitzuak = _zerbitzuRepo.GetAll()
                .Where(z => z.ErreserbaId.HasValue && z.ErreserbaId.Value == id)
                .ToList();

            foreach (var z in lotutakoZerbitzuak)
            {
                z.ErreserbaId = null;
                _zerbitzuRepo.Update(z);
            }

            _repo.Delete(e);
            return Ok(new { mezua = "Ezabatuta" });
        }
    }
}
