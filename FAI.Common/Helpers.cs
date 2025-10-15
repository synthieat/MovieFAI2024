using FAI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Common
{
    public abstract class Helpers
    {
        public static void MapEntityProperties<TSource, TTarget>(TSource source, TTarget target, List<string> exlcudeProperties)
            where TSource : class
            where TTarget : class, IEntity
        {
            var sourceType = source.GetType();
            var targetType = target.GetType();  

            if (sourceType.BaseType.FullName != targetType.BaseType.FullName)
            {
                throw new ApplicationException("Source and Target must be of the same base type");
            }

            var targetPropertes = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

            targetPropertes.ForEach(p =>
            {
                if(p.CanWrite && !(exlcudeProperties ?? []).Contains(p.Name))
                {
                   /* Passende Property aus Quelle (Source) lesen */
                   var sourceProperty = sourceType.GetProperty(p.Name, BindingFlags.Public | BindingFlags.Instance);
                    if (sourceProperty != null)
                    {
                        /* Property Wert aus Quelle (Source) lesen */
                        var sourcePropertyValue = sourceProperty.GetValue(source, null);

                        /* Ausgelesener Wert ins Ziel (Target) schreiben */
                        p.SetValue(target, sourcePropertyValue, null);
                    }
                }

            });
        }

    }
}
