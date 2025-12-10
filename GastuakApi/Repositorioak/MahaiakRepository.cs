using JatetxeaApi.Modeloak;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using NHSession = NHibernate.ISession;
using NHSessionFactory = NHibernate.ISessionFactory;

namespace JatetxeaApi.Repositorioak
{
    public class MahaiakRepository
    {
        private readonly NHSession _session;

        public MahaiakRepository(NHSessionFactory sessionFactory)
        {
            _session = sessionFactory.GetCurrentSession();
        }

        public void Add(Mahaiak item)
        {
            using var tx = _session.BeginTransaction();
            _session.Save(item);
            tx.Commit();
        }

        public Mahaiak? Get(int id)
        {
            return _session.Query<Mahaiak>().SingleOrDefault(x => x.Id == id);
        }

        public IList<Mahaiak> GetAll()
        {
            return _session.Query<Mahaiak>().ToList();
        }

        public void Update(Mahaiak item)
        {
            using var tx = _session.BeginTransaction();
            _session.Update(item);
            tx.Commit();
        }

        public void Delete(Mahaiak item)
        {
            using var tx = _session.BeginTransaction();
            _session.Delete(item);
            tx.Commit();
        }
    }
}
