using System.Linq;
using Web.Infrastructure.DataTables.Model;

namespace Web.Infrastructure.DataTables.Helpers
{
    public static class CustomTableFormattingUtils
    {
        public static CustomResultSet<TModel> MapResults<TEntity, TModel>(CustomResultSet<TEntity> results) where TModel : IModel<TModel, TEntity>, new()
        {
            return new CustomResultSet<TModel>
            {
                CurrentPage = results.CurrentPage,
                PageSize = results.PageSize,
                RecordCount = results.RecordCount,
                aaData = results.aaData.Select(c => new TModel().MapEntity(c)).ToList(),
                sEcho = results.sEcho
            };
        }
    }
}
