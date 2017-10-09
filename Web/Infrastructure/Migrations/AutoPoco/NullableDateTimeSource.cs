using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoPoco.Engine;
using Core;

namespace Web.Infrastructure.Migrations.AutoPoco
{
    public class NullableDateTimeSource : DatasourceBase<DateTime?>
    {
        public override DateTime? Next(IGenerationContext context)
        {
            return AppTime.Now();
        }
    }
}
