using JatetxeaApi.Modeloak;
using JatetxeaApi.DTOak;
using JatetxeaApi.Repositorioak;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace JatetxeaApi.Controllerrak
{
    [ApiController]
    [Route("api/[controller]")]
    public class LangileakController : ControllerBase
    {
        private readonly LangileakRepository _repo;

        public LangileakController(LangileakRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var lista = _repo.GetAll().Select(l => new LangileakDto
            {
                Id = l.Id,
                Izena = l.Izena,
                Erabiltzailea = l.Erabiltzailea,
                Pasahitza = l.Pasahitza,
                Aktibo = l.Aktibo,
                ErregistroData = l.ErregistroData,
                RolaId = l.RolaId
            });

            return Ok(lista);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var l = _repo.Get(id);
            if (l == null) return NotFound(new { mezua = "Ez da aurkitu" });

            return Ok(new LangileakDto
            {
                Id = l.Id,
                Izena = l.Izena,
                Erabiltzailea = l.Erabiltzailea,
                Pasahitza = l.Pasahitza,
                Aktibo = l.Aktibo,
                ErregistroData = l.ErregistroData,
                RolaId = l.RolaId
            });
        }

        [HttpPost]
        public IActionResult Sortu([FromBody] LangileakSortuDto dto)
        {
            var l = new Langileak(dto.Izena, dto.Erabiltzailea, dto.Pasahitza, dto.Aktibo, dto.ErregistroData, dto.RolaId);
            _repo.Add(l);
            return Ok(new { mezua = "Langilea sortuta", id = l.Id });
        }

        [HttpPut("{id}")]
        public IActionResult Eguneratu(int id, [FromBody] LangileakSortuDto dto)
        {
            var l = _repo.Get(id);
            if (l == null) return NotFound(new { mezua = "Ez da aurkitu" });

            l.Izena = dto.Izena;
            l.Erabiltzailea = dto.Erabiltzailea;
            l.Pasahitza = dto.Pasahitza;
            l.Aktibo = dto.Aktibo;
            l.ErregistroData = dto.ErregistroData;
            l.RolaId = dto.RolaId;

            _repo.Update(l);
            return Ok(new { mezua = "Eguneratuta" });
        }

        [HttpDelete("{id}")]
        public IActionResult Ezabatu(int id)
        {
            var l = _repo.Get(id);
            if (l == null) return NotFound(new { mezua = "Ez da aurkitu" });

            _repo.Delete(l);
            return Ok(new { mezua = "Ezabatuta" });
        }
    }
}
