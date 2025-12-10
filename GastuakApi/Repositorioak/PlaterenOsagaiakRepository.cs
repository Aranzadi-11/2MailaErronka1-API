using JatetxeaApi.Modeloak;
using NHibernate;
using System.Linq;
using System.Collections.Generic;

using NHSession = NHibernate.ISession;
using NHSessionFactory = NHibernate.ISessionFactory;

namespace JatetxeaApi.Repositorioak
{
    public class PlaterenOsagaiakRepository
    {
        private readonly NHSession _session;

        public PlaterenOsagaiakRepository(NHSessionFactory sessionFactory)
        {
            _session = sessionFactory.GetCurrentSession();
        }

        public void Add(PlaterenOsagaiak item)
        {
            using var tx = _session.BeginTransaction();
            _session.Save(item);
            tx.Commit();
        }

        public PlaterenOsagaiak? Get(int plateraId, int inbentarioaId)
        {
            return _session.Query<PlaterenOsagaiak>()
                .SingleOrDefault(x =>
                    x.PlateraId == plateraId &&
                    x.InbentarioaId == inbentarioaId);
        }

        public IList<PlaterenOsagaiak> GetAll()
        {
            return _session.Query<PlaterenOsagaiak>().ToList();
        }

        public void Update(PlaterenOsagaiak item)
        {
            using var tx = _session.BeginTransaction();
            _session.Update(item);
            tx.Commit();
        }

        public void Delete(PlaterenOsagaiak item)
        {
            using var tx = _session.BeginTransaction();
            _session.Delete(item);
            tx.Commit();
        }
    }
}
