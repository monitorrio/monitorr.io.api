using System;
using Core;
using Core.Domain;
using Web.Models;

namespace Web.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static Log ToNewEntity(this LogModel m)
        {
            var x = new Log
            {
                DateCreated = AppTime.Now(),
                Name = m.Name,
                LogId = Guid.NewGuid().ToString(),
                WidgetColor = m.WidgetColor,
                ShortName = m.Name.GetRandomLetters(2)               
            };

            return x;
        }
    }
}
