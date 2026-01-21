using JatetxeaApi.Repositorioak;
using JatetxeaApi.Modeloak;
using JatetxeaApi.DTOak;
using Microsoft.AspNetCore.Mvc;

namespace JatetxeaApi.Controllerrak
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZerbitzuXehetasunakController : ControllerBase
    {
        private readonly ZerbitzuXehetasunakRepository _repo;

        public ZerbitzuXehetasunakController(ZerbitzuXehetasunakRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var elementuak = _repo.GetAll();

            var dtoLista = elementuak.Select(e => new ZerbitzuXehetasunakDto
            {
                Id = e.Id,
                ZerbitzuaId = e.ZerbitzuaId,
                PlateraId = e.PlateraId,
                Kantitatea = e.Kantitatea,
                PrezioUnitarioa = e.PrezioUnitarioa,
                Zerbitzatuta = e.Zerbitzatuta
            });

            return Ok(dtoLista);
        }

        [HttpPost]
        public IActionResult Sortu([FromBody] ZerbitzuXehetasunakSortuDto dto)
        {
            var elementua = new ZerbitzuXehetasunak
            {
                ZerbitzuaId = dto.ZerbitzuaId,
                PlateraId = dto.PlateraId,
                Kantitatea = dto.Kantitatea,
                PrezioUnitarioa = dto.PrezioUnitarioa,
                Zerbitzatuta = dto.Zerbitzatuta
            };

            _repo.Add(elementua);

            return Ok(new
            {
                mezua = "Xehetasuna sortuta",
                id = elementua.Id
            });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var e = _repo.Get(id);
            if (e == null)
                return NotFound(new { mezua = "Ez da aurkitu" });

            var dto = new ZerbitzuXehetasunakDto
            {
                Id = e.Id,
                ZerbitzuaId = e.ZerbitzuaId,
                PlateraId = e.PlateraId,
                Kantitatea = e.Kantitatea,
                PrezioUnitarioa = e.PrezioUnitarioa,
                Zerbitzatuta = e.Zerbitzatuta
            };

            return Ok(dto);
        }

        [HttpPut("{id}")]
        public IActionResult Eguneratu(int id, [FromBody] ZerbitzuXehetasunakSortuDto dto)
        {
            var e = _repo.Get(id);
            if (e == null)
                return NotFound(new { mezua = "Ez da aurkitu" });

            e.ZerbitzuaId = dto.ZerbitzuaId;
            e.PlateraId = dto.PlateraId;
            e.Kantitatea = dto.Kantitatea;
            e.PrezioUnitarioa = dto.PrezioUnitarioa;
            e.Zerbitzatuta = dto.Zerbitzatuta;

            _repo.Update(e);

            return Ok(new { mezua = "Eguneratuta" });
        }

        [HttpDelete("{id}")]
        public IActionResult Ezabatu(int id)
        {
            var e = _repo.Get(id);
            if (e == null)
                return NotFound(new { mezua = "Ez da aurkitu" });

            _repo.Delete(e);

            return Ok(new { mezua = "Ezabatuta" });
        }
    }
}