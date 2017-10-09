
namespace Web.Infrastructure.DataTables.Model
{
    public interface IModel<TModel, TEntity>
    {
        TModel MapEntity(TEntity entity);
    }
}
