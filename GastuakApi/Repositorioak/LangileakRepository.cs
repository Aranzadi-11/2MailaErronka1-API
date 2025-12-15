using JatetxeaApi.Modeloak;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using NHSession = NHibernate.ISession;
using NHSessionFactory = NHibernate.ISessionFactory;

namespace JatetxeaApi.Repositorioak
{
    public class LangileakRepository
    {
        private readonly NHSession _session;

        public LangileakRepository(NHSessionFactory sessionFactory)
        {
            _session = sessionFactory.GetCurrentSession();
        }

        public void Add(Langileak item)
        {
            using var tx = _session.BeginTransaction();
            _session.Save(item);
            tx.Commit();
        }

        public Langileak? Get(int id)
        {
            return _session.Query<Langileak>().SingleOrDefault(x => x.Id == id);
        }

        public Langileak? Get(string izena)
        {
            return _session.Query<Langileak>().SingleOrDefault(x => x.Izena == izena);
        }

        public IList<Langileak> GetAll()
        {
            return _session.Query<Langileak>().ToList();
        }

        public void Update(Langileak item)
        {
            using var tx = _session.BeginTransaction();
            _session.Update(item);
            tx.Commit();
        }

        public void Delete(Langileak item)
        {
            using var tx = _session.BeginTransaction();
            _session.Delete(item);
            tx.Commit();
        }
    }
}
