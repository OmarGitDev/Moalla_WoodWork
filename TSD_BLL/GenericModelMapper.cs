using System;
using System.Collections.Generic;
using System.Reflection;

namespace TSD_BLL
{
    public class GenericModelMapper
    {
        public static DestinationType GetModel<DestinationType, SourceType>(SourceType SourceObject)
        {
            object DestinationObject = Activator.CreateInstance(typeof(DestinationType));
            if (SourceObject != null)
            {
                foreach (PropertyInfo SourcePInfo in typeof(SourceType).GetProperties())
                {
                    PropertyInfo DestinationPInfo = typeof(DestinationType).GetProperty(SourcePInfo.Name);
                    if (DestinationPInfo != null)
                    {
                        try
                        {
                            DestinationPInfo.SetValue(DestinationObject, SourcePInfo.GetValue(SourceObject));
                        }
                        catch
                        {
                            //Last try --> conversion 
                            try
                            {
                                DestinationPInfo.SetValue(DestinationObject, Convert.ChangeType(SourcePInfo.GetValue(SourceObject), DestinationPInfo.PropertyType));
                            }
                            catch
                            {
                                //don nothing when an exception is raised at this level
                                //continue with the rest of the properties  
                                continue;
                            }
                            continue;
                        }
                    }
                }
            }
            return ((DestinationType)Convert.ChangeType(DestinationObject, typeof(DestinationType)));
        }
        public static List<DestinationType> GetModelList<DestinationType, SourceType>(List<SourceType> SourceObject)
        {
            List<DestinationType> _DestinationType = new List<DestinationType>();
            foreach (var item in SourceObject)
            {
                _DestinationType.Add(GetModel<DestinationType, SourceType>(item));
            }
            return _DestinationType;
        }
    }
}
