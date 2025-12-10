using JatetxeaApi.Modeloak;
using JatetxeaApi.DTOak;
using JatetxeaApi.Repositorioak;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace JatetxeaApi.Controllerrak
{
    [ApiController]
    [Route("api/[controller]")]
    public class JatetxekoInfoController : ControllerBase
    {
        private readonly JatetxekoInfoRepository _repo;

        public JatetxekoInfoController(JatetxekoInfoRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var lista = _repo.GetAll().Select(j => new JatetxekoInfoDto
            {
                Id = j.Id,
                Izena = j.Izena,
                KaxaTotal = j.KaxaTotal,
                Helbidea = j.Helbidea,
                TelefonoZenbakia = j.TelefonoZenbakia
            });

            return Ok(lista);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var j = _repo.Get(id);
            if (j == null) return NotFound(new { mezua = "Ez da aurkitu" });

            return Ok(new JatetxekoInfoDto
            {
                Id = j.Id,
                Izena = j.Izena,
                KaxaTotal = j.KaxaTotal,
                Helbidea = j.Helbidea,
                TelefonoZenbakia = j.TelefonoZenbakia
            });
        }

        [HttpPost]
        public IActionResult Sortu([FromBody] JatetxekoInfoSortuDto dto)
        {
            var j = new JatetxekoInfo(dto.Izena, dto.KaxaTotal, dto.Helbidea, dto.TelefonoZenbakia);
            _repo.Add(j);
            return Ok(new { mezua = "Informazioa sortuta", id = j.Id });
        }

        [HttpPut("{id}")]
        public IActionResult Eguneratu(int id, [FromBody] JatetxekoInfoSortuDto dto)
        {
            var j = _repo.Get(id);
            if (j == null) return NotFound(new { mezua = "Ez da aurkitu" });

            j.Izena = dto.Izena;
            j.KaxaTotal = dto.KaxaTotal;
            j.Helbidea = dto.Helbidea;
            j.TelefonoZenbakia = dto.TelefonoZenbakia;

            _repo.Update(j);
            return Ok(new { mezua = "Eguneratuta" });
        }

        [HttpDelete("{id}")]
        public IActionResult Ezabatu(int id)
        {
            var j = _repo.Get(id);
            if (j == null) return NotFound(new { mezua = "Ez da aurkitu" });

            _repo.Delete(j);
            return Ok(new { mezua = "Ezabatuta" });
        }
    }
}
