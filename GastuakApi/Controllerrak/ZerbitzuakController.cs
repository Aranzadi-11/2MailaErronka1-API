using JatetxeaApi.Modeloak;
using JatetxeaApi.DTOak;
using JatetxeaApi.Repositorioak;
using Microsoft.AspNetCore.Mvc;
using System;
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
                ErreserbaId = z.ErreserbaId,
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
                ErreserbaId = z.ErreserbaId,
                EskaeraData = z.EskaeraData,
                Egoera = z.Egoera,
                Guztira = z.Guztira
            });
        }

        [HttpPost]
        public IActionResult Sortu([FromBody] ZerbitzuakSortuDto dto)
        {
            var z = new Zerbitzuak(dto.LangileId, dto.MahaiaId, dto.ErreserbaId, DateTime.Now, dto.Egoera, dto.Guztira);
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
            z.ErreserbaId = dto.ErreserbaId;

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

        [HttpPost("egin")]
        public IActionResult ZerbitzuaEgin([FromBody] ZerbitzuaEskariaDto dto)
        {
            using var session = NHibernateHelper.SessionFactory.OpenSession();
            using var tx = session.BeginTransaction();

            try
            {
                var zerbitzua = session.Query<Zerbitzuak>()
                    .FirstOrDefault(z => z.ErreserbaId == dto.ErreserbaId);

                if (zerbitzua == null)
                {
                    zerbitzua = new Zerbitzuak(dto.LangileId, dto.MahaiaId, dto.ErreserbaId, DateTime.Now, "Itxaropean", 0);
                    session.Save(zerbitzua);
                }

                var xehetasunak = session.Query<ZerbitzuXehetasunak>()
                    .Where(x => x.ZerbitzuaId == zerbitzua.Id)
                    .ToList();

                foreach (var p in dto.Platerak)
                {
                    var zaharra = xehetasunak.FirstOrDefault(x => x.PlateraId == p.PlateraId);
                    var berriaKant = p.Kantitatea;

                    if (zaharra == null && berriaKant > 0)
                    {
                        var platera = session.Get<Platerak>(p.PlateraId);
                        var osagaiak = session.Query<PlaterenOsagaiak>()
                            .Where(o => o.PlateraId == p.PlateraId)
                            .ToList();

                        foreach (var o in osagaiak)
                        {
                            var inv = session.Get<Inbentarioa>(o.InbentarioaId);
                            session.Lock(inv, NHibernate.LockMode.Upgrade);
                            inv.Kantitatea -= (int)(o.Kantitatea * berriaKant);
                            inv.AzkenEguneratzea = DateTime.Now;
                            session.Update(inv);
                        }

                        session.Save(new ZerbitzuXehetasunak
                        {
                            ZerbitzuaId = zerbitzua.Id,
                            PlateraId = p.PlateraId,
                            Kantitatea = berriaKant,
                            PrezioUnitarioa = platera.Prezioa,
                            Zerbitzatuta = false
                        });

                        continue;
                    }

                    if (zaharra != null)
                    {
                        if (zaharra.Zerbitzatuta && berriaKant < zaharra.Kantitatea)
                            berriaKant = zaharra.Kantitatea;

                        var diferentzia = berriaKant - zaharra.Kantitatea;

                        if (diferentzia > 0)
                        {
                            var osagaiak = session.Query<PlaterenOsagaiak>()
                                .Where(o => o.PlateraId == p.PlateraId)
                                .ToList();

                            foreach (var o in osagaiak)
                            {
                                var inv = session.Get<Inbentarioa>(o.InbentarioaId);
                                session.Lock(inv, NHibernate.LockMode.Upgrade);
                                inv.Kantitatea -= (int)(o.Kantitatea * diferentzia);
                                inv.AzkenEguneratzea = DateTime.Now;
                                session.Update(inv);
                            }

                            zaharra.Kantitatea = berriaKant;
                            session.Update(zaharra);
                        }
                        else if (diferentzia < 0 && !zaharra.Zerbitzatuta)
                        {
                            var osagaiak = session.Query<PlaterenOsagaiak>()
                                .Where(o => o.PlateraId == p.PlateraId)
                                .ToList();

                            foreach (var o in osagaiak)
                            {
                                var inv = session.Get<Inbentarioa>(o.InbentarioaId);
                                session.Lock(inv, NHibernate.LockMode.Upgrade);
                                inv.Kantitatea += (int)(o.Kantitatea * -diferentzia);
                                inv.AzkenEguneratzea = DateTime.Now;
                                session.Update(inv);
                            }

                            zaharra.Kantitatea = berriaKant;
                            session.Update(zaharra);
                        }

                        if (zaharra.Kantitatea == 0 && !zaharra.Zerbitzatuta)
                        {
                            session.Delete(zaharra);
                        }
                    }
                }

                var xeheList = session.Query<ZerbitzuXehetasunak>()
                    .Where(x => x.ZerbitzuaId == zerbitzua.Id)
                    .ToList();

                zerbitzua.Guztira = xeheList.Any()
                    ? xeheList.Sum(x => x.PrezioUnitarioa * x.Kantitatea)
                    : 0;



                session.Update(zerbitzua);
                tx.Commit();

                return Ok(new ZerbitzuaEmaitzaDto
                {
                    Ondo = true,
                    ZerbitzuaId = zerbitzua.Id,
                    Erroreak = new()
                });
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        [HttpGet("erreserba/{erreserbaId}/platerak")]
        public IActionResult GetPlaterakByErreserba(int erreserbaId)
        {
            var zerbitzua = _repo.GetAll().FirstOrDefault(z => z.ErreserbaId == erreserbaId);
            if (zerbitzua == null) return NotFound(new { mezua = "Ez dago zerbitzurik erreserba honekin" });

            using var session = NHibernateHelper.SessionFactory.OpenSession();
            var xehetasunak = session.Query<ZerbitzuXehetasunak>()
                .Where(x => x.ZerbitzuaId == zerbitzua.Id)
                .Select(x => new
                {
                    x.PlateraId,
                    x.Kantitatea,
                    x.Zerbitzatuta
                })
                .ToList();

            return Ok(xehetasunak);
        }
    }
}
