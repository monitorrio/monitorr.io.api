using System.Linq;
using Web.Infrastructure.DataTables.Model;

namespace Web.Infrastructure.DataTables.Helpers
{
    public static class TableFormatting
    {
        public static ResultSet<TModel> MapResults<TEntity, TModel>(ResultSet<TEntity> results) where TModel : IModel<TModel, TEntity>, new()
        {
            return new ResultSet<TModel>
            {
                CurrentPage = results.CurrentPage,
                PageSize = results.PageSize,
                RecordCount = results.RecordCount,
                Items = results.Items.Select(c => new TModel().MapEntity(c)).ToList(),
                sEcho = results.sEcho
            };
        }
    }
}
