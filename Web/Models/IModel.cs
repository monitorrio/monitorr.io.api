namespace Web.Models
{
    public interface IModel<TModel, TEntity>
    {
        TModel MapEntity(TEntity entity);
    }

    public static class ModelExtensions
    {
        public static TModel MapEntity<TModel, TEntity>(this TModel model, TEntity entity) where TModel : IModel<TModel, TEntity>
        {
            return model.MapEntity(entity);
        }
    }
}
