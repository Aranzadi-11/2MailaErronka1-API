using JatetxeaApi.Repositorioak;
using JatetxeaApi.Modeloak;
using JatetxeaApi.DTOak;
using Microsoft.AspNetCore.Mvc;

namespace JatetxeaApi.Controllerrak
{
    [ApiController]
    [Route("api/[controller]")]
    public class InbentarioaController : ControllerBase
    {
        private readonly InbentarioaRepository _repo;

        public InbentarioaController(InbentarioaRepository repo)
        {
            _repo = repo;
        }

        // GET api/inbentarioa
        [HttpGet]
        public IActionResult GetAll()
        {
            var elementuak = _repo.GetAll();

            var dtoLista = elementuak.Select(e => new InbentarioaDto
            {
                Id = e.Id,
                Izena = e.Izena,
                Deskribapena = e.Deskribapena,
                Kantitatea = e.Kantitatea,
                NeurriaUnitatea = e.NeurriaUnitatea,
                StockMinimoa = e.StockMinimoa,
                AzkenEguneratzea = e.AzkenEguneratzea
            });

            return Ok(dtoLista);
        }

        // POST api/inbentarioa
        [HttpPost]
        public IActionResult Sortu([FromBody] InbentarioaSortuDto dto)
        {
            var elementua = new Inbentarioa
            {
                Izena = dto.Izena,
                Deskribapena = dto.Deskribapena,
                Kantitatea = dto.Kantitatea,
                NeurriaUnitatea = dto.NeurriaUnitatea,
                StockMinimoa = dto.StockMinimoa,
                AzkenEguneratzea = DateTime.Now
            };

            _repo.Add(elementua);

            return Ok(new
            {
                mezua = "Elementua sortuta",
                id = elementua.Id
            });
        }

        // GET api/inbentarioa/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var e = _repo.Get(id);
            if (e == null)
                return NotFound(new { mezua = "Ez da aurkitu" });

            var dto = new InbentarioaDto
            {
                Id = e.Id,
                Izena = e.Izena,
                Deskribapena = e.Deskribapena,
                Kantitatea = e.Kantitatea,
                NeurriaUnitatea = e.NeurriaUnitatea,
                StockMinimoa = e.StockMinimoa,
                AzkenEguneratzea = e.AzkenEguneratzea
            };

            return Ok(dto);
        }

        // PUT api/inbentarioa/{id}
        [HttpPut("{id}")]
        public IActionResult Eguneratu(int id, [FromBody] InbentarioaSortuDto dto)
        {
            var e = _repo.Get(id);
            if (e == null)
                return NotFound(new { mezua = "Ez da aurkitu" });

            e.Izena = dto.Izena;
            e.Deskribapena = dto.Deskribapena;
            e.Kantitatea = dto.Kantitatea;
            e.NeurriaUnitatea = dto.NeurriaUnitatea;
            e.StockMinimoa = dto.StockMinimoa;
            e.AzkenEguneratzea = DateTime.Now;

            _repo.Update(e);

            return Ok(new { mezua = "Eguneratuta" });
        }

        // PATCH api/inbentarioa/{id}
        [HttpPatch("{id}")]
        public IActionResult EguneratuZatia(int id, [FromBody] InbentarioaPatchDto dto)
        {
            var e = _repo.Get(id);
            if (e == null)
                return NotFound(new { mezua = "Ez da aurkitu" });

            if (dto.Izena != null) e.Izena = dto.Izena;
            if (dto.Deskribapena != null) e.Deskribapena = dto.Deskribapena;
            if (dto.Kantitatea.HasValue) e.Kantitatea = dto.Kantitatea.Value;
            if (dto.NeurriaUnitatea != null) e.NeurriaUnitatea = dto.NeurriaUnitatea;
            if (dto.StockMinimoa.HasValue) e.StockMinimoa = dto.StockMinimoa.Value;

            e.AzkenEguneratzea = DateTime.Now;

            _repo.Update(e);

            return Ok(new { mezua = "Zati batean eguneratuta" });
        }

        // DELETE api/inbentarioa/{id}
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