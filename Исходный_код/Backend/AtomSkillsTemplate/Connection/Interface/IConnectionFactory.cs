using System.Data;

namespace AtomSkillsTemplate.Connection.Interface
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection();
    }
}
