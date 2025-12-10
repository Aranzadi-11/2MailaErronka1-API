using JatetxeaApi.Modeloak;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using NHSession = NHibernate.ISession;
using NHSessionFactory = NHibernate.ISessionFactory;

namespace JatetxeaApi.Repositorioak
{
    public class ZerbitzuXehetasunakRepository
    {
        private readonly NHSession _session;

        public ZerbitzuXehetasunakRepository(NHSessionFactory sessionFactory)
        {
            _session = sessionFactory.GetCurrentSession();
        }

        public void Add(ZerbitzuXehetasunak item)
        {
            using var tx = _session.BeginTransaction();
            _session.Save(item);
            tx.Commit();
        }

        public ZerbitzuXehetasunak? Get(int id)
        {
            return _session.Query<ZerbitzuXehetasunak>().SingleOrDefault(x => x.Id == id);
        }

        public IList<ZerbitzuXehetasunak> GetAll()
        {
            return _session.Query<ZerbitzuXehetasunak>().ToList();
        }

        public void Update(ZerbitzuXehetasunak item)
        {
            using var tx = _session.BeginTransaction();
            _session.Update(item);
            tx.Commit();
        }

        public void Delete(ZerbitzuXehetasunak item)
        {
            using var tx = _session.BeginTransaction();
            _session.Delete(item);
            tx.Commit();
        }
    }
}
