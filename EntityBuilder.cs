using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig.Entity;
using Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper
{
    public static class EntityBuilder
    {
        private static readonly Type s_RootType = typeof(object);
        private static Dictionary<Type, Dictionary<string, EntityBuilder.PropertyDataBindingInfo>> s_TypeMappingInfo = new Dictionary<Type, Dictionary<string, EntityBuilder.PropertyDataBindingInfo>>();
        private static Dictionary<Type, List<EntityBuilder.ReferencedTypeBindingInfo>> s_TypeReferencedList = new Dictionary<Type, List<EntityBuilder.ReferencedTypeBindingInfo>>();
        private static Dictionary<Type, Dictionary<string, DataMappingAttribute>> s_TypePropertyInfo = new Dictionary<Type, Dictionary<string, DataMappingAttribute>>();
        private static object s_SyncMappingInfo = new object();

        public static T BuildEntity<T>(IDataReader dr) where T : class, new()
        {
            return EntityBuilder.BuildEntity<T>((IEntityDataSource)new DataReaderEntitySource(dr), string.Empty);
        }

        public static object BuildEntity(IDataReader dr, Type type)
        {
            return EntityBuilder.BuildEntity((IEntityDataSource)new DataReaderEntitySource(dr), type, string.Empty);
        }

        public static T BuildEntity<T>(DataRow dr) where T : class, new()
        {
            return EntityBuilder.BuildEntity<T>((IEntityDataSource)new DataRowEntitySource(dr), string.Empty);
        }

        public static object BuildEntity(DataRow dr, Type type)
        {
            return EntityBuilder.BuildEntity((IEntityDataSource)new DataRowEntitySource(dr), type, string.Empty);
        }

        public static List<T> BuildEntityList<T>(DataRow[] rows) where T : class, new()
        {
            if (rows == null)
                return new List<T>(0);
            List<T> list = new List<T>(rows.Length);
            foreach (DataRow dr in rows)
                list.Add(EntityBuilder.BuildEntity<T>(dr));
            return list;
        }

        public static List<T> BuildEntityList<T>(DataTable table) where T : class, new()
        {
            if (table == null)
                return new List<T>(0);
            List<T> list = new List<T>(table.Rows.Count);
            foreach (DataRow dr in (InternalDataCollectionBase)table.Rows)
                list.Add(EntityBuilder.BuildEntity<T>(dr));
            return list;
        }

        private static T BuildEntity<T>(IEntityDataSource ds, string prefix) where T : class, new()
        {
            T instance = Activator.CreateInstance<T>();
            EntityBuilder.FillEntity(ds, (object)instance, typeof(T), prefix);
            return instance;
        }

        private static object BuildEntity(IEntityDataSource ds, Type type, string prefix)
        {
            object instance = Activator.CreateInstance(type);
            EntityBuilder.FillEntity(ds, instance, type, prefix);
            return instance;
        }

        private static void FillEntity(IEntityDataSource ds, object obj, Type type, string prefix)
        {
            Type baseType = type.BaseType;
            if (!EntityBuilder.s_RootType.Equals(baseType))
                EntityBuilder.FillEntity(ds, obj, baseType, prefix);
            EntityBuilder.DoFillEntity(ds, obj, type, prefix);
        }

        private static void DoFillEntity(IEntityDataSource ds, object obj, Type type, string prefix)
        {
            foreach (string index in (IEnumerable<string>)ds)
            {
                string columnName = index.ToUpper();
                if (!string.IsNullOrEmpty(prefix) && columnName.StartsWith(prefix.ToUpper()))
                    columnName = columnName.Substring(prefix.Length);
                if (string.IsNullOrEmpty(prefix))
                    prefix = string.Empty;
                EntityBuilder.PropertyDataBindingInfo propertyInfo = EntityBuilder.GetPropertyInfo(type, columnName);
                if (propertyInfo != null && !(index.ToUpper() != prefix.ToUpper() + columnName) && (ds[index] != DBNull.Value && EntityBuilder.ValidateData(propertyInfo, ds[index])))
                {
                    object obj1 = ds[index];
                    Type propertyType1 = propertyInfo.PropertyInfo.PropertyType;
                    if (propertyType1 == typeof(string))
                        obj1 = (object)obj1.ToString().Trim();
                    else if (propertyType1.FullName.StartsWith("System.Nullable`1"))
                    {
                        Type propertyType2 = propertyType1.GetProperty("Value").PropertyType;
                        if (obj1 != null && propertyType2.IsEnum)
                            obj1 = propertyType1.GetConstructor(new Type[1]
              {
                propertyType2
              }).Invoke(new object[1]
              {
                obj1
              });
                    }
                    propertyInfo.PropertyInfo.SetValue(obj, obj1, (object[])null);
                }
            }
            foreach (EntityBuilder.ReferencedTypeBindingInfo refObj in EntityBuilder.GetReferenceObjects(type))
            {
                if (EntityBuilder.TryFill(ds, refObj))
                    refObj.PropertyInfo.SetValue(obj, EntityBuilder.BuildEntity(ds, refObj.Type, refObj.Prefix), (object[])null);
            }
        }

        private static bool TryFill(IEntityDataSource ds, EntityBuilder.ReferencedTypeBindingInfo refObj)
        {
            if (string.IsNullOrEmpty(refObj.ConditionalProperty))
                return true;
            string bindingColumnName = EntityBuilder.GetBindingColumnName(refObj.Type, refObj.ConditionalProperty, refObj.Prefix);
            if (bindingColumnName == null)
                return false;
            else
                return ds.ContainsColumn(bindingColumnName);
        }

        private static string GetBindingColumnName(Type type, string propertyName, string prefix)
        {
            string str;
            try
            {
                Dictionary<string, DataMappingAttribute> dictionary;
                EntityBuilder.s_TypePropertyInfo.TryGetValue(type, out dictionary);
                if (dictionary == null)
                {
                    lock (EntityBuilder.s_SyncMappingInfo)
                    {
                        EntityBuilder.s_TypePropertyInfo.TryGetValue(type, out dictionary);
                        if (dictionary == null)
                        {
                            EntityBuilder.AddTypeInfo(type);
                            dictionary = EntityBuilder.s_TypePropertyInfo[type];
                        }
                    }
                }
                DataMappingAttribute mappingAttribute;
                dictionary.TryGetValue(propertyName, out mappingAttribute);
                str = mappingAttribute != null ? mappingAttribute.ColumnName : (string)null;
            }
            catch
            {
                str = (string)null;
            }
            if (str != null)
                return prefix + str;
            if (!EntityBuilder.s_RootType.Equals(type.BaseType) && !EntityBuilder.s_RootType.Equals(type))
                return EntityBuilder.GetBindingColumnName(type.BaseType, propertyName, prefix);
            else
                return (string)null;
        }

        private static bool ValidateData(EntityBuilder.PropertyDataBindingInfo bindingInfo, object dbValue)
        {
            return true;
        }

        private static EntityBuilder.PropertyDataBindingInfo GetPropertyInfo(Type type, string columnName)
        {
            Dictionary<string, EntityBuilder.PropertyDataBindingInfo> dictionary;
            try
            {
                EntityBuilder.s_TypeMappingInfo.TryGetValue(type, out dictionary);
                if (dictionary == null)
                {
                    lock (EntityBuilder.s_SyncMappingInfo)
                    {
                        EntityBuilder.s_TypeMappingInfo.TryGetValue(type, out dictionary);
                        if (dictionary == null)
                        {
                            EntityBuilder.AddTypeInfo(type);
                            dictionary = EntityBuilder.s_TypeMappingInfo[type];
                        }
                    }
                }
            }
            catch
            {
                return (EntityBuilder.PropertyDataBindingInfo)null;
            }
            EntityBuilder.PropertyDataBindingInfo propertyDataBindingInfo;
            dictionary.TryGetValue(columnName, out propertyDataBindingInfo);
            return propertyDataBindingInfo;
        }

        private static List<EntityBuilder.ReferencedTypeBindingInfo> GetReferenceObjects(Type type)
        {
            List<EntityBuilder.ReferencedTypeBindingInfo> list;
            EntityBuilder.s_TypeReferencedList.TryGetValue(type, out list);
            if (list == null)
            {
                lock (EntityBuilder.s_SyncMappingInfo)
                {
                    EntityBuilder.s_TypeReferencedList.TryGetValue(type, out list);
                    if (list == null)
                    {
                        EntityBuilder.AddTypeInfo(type);
                        list = EntityBuilder.s_TypeReferencedList[type];
                    }
                }
            }
            return list;
        }

        private static void GetTypeInfo(Type type, out Dictionary<string, EntityBuilder.PropertyDataBindingInfo> dataMappingInfos, out List<EntityBuilder.ReferencedTypeBindingInfo> referObjs, out Dictionary<string, DataMappingAttribute> propertyInfos)
        {
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            dataMappingInfos = new Dictionary<string, EntityBuilder.PropertyDataBindingInfo>();
            referObjs = new List<EntityBuilder.ReferencedTypeBindingInfo>();
            propertyInfos = new Dictionary<string, DataMappingAttribute>((IEqualityComparer<string>)new CaseInsensitiveStringEqualityComparer());
            foreach (PropertyInfo propertyInfo in properties)
            {
                foreach (object obj in propertyInfo.GetCustomAttributes(false))
                {
                    if (obj is DataMappingAttribute)
                    {
                        DataMappingAttribute mapping = obj as DataMappingAttribute;
                        dataMappingInfos[mapping.ColumnName.ToUpper()] = new EntityBuilder.PropertyDataBindingInfo(mapping, propertyInfo);
                        propertyInfos.Add(propertyInfo.Name, mapping);
                    }
                    else if (obj is ReferencedEntityAttribute)
                    {
                        ReferencedEntityAttribute attri = obj as ReferencedEntityAttribute;
                        referObjs.Add(new EntityBuilder.ReferencedTypeBindingInfo(attri, propertyInfo));
                    }
                }
            }
        }

        private static void AddTypeInfo(Type type)
        {
            Dictionary<Type, Dictionary<string, EntityBuilder.PropertyDataBindingInfo>> dictionary1 = new Dictionary<Type, Dictionary<string, EntityBuilder.PropertyDataBindingInfo>>((IDictionary<Type, Dictionary<string, EntityBuilder.PropertyDataBindingInfo>>)EntityBuilder.s_TypeMappingInfo);
            Dictionary<Type, List<EntityBuilder.ReferencedTypeBindingInfo>> dictionary2 = new Dictionary<Type, List<EntityBuilder.ReferencedTypeBindingInfo>>((IDictionary<Type, List<EntityBuilder.ReferencedTypeBindingInfo>>)EntityBuilder.s_TypeReferencedList);
            Dictionary<Type, Dictionary<string, DataMappingAttribute>> dictionary3 = new Dictionary<Type, Dictionary<string, DataMappingAttribute>>((IDictionary<Type, Dictionary<string, DataMappingAttribute>>)EntityBuilder.s_TypePropertyInfo);
            Dictionary<string, EntityBuilder.PropertyDataBindingInfo> dataMappingInfos;
            List<EntityBuilder.ReferencedTypeBindingInfo> referObjs;
            Dictionary<string, DataMappingAttribute> propertyInfos;
            EntityBuilder.GetTypeInfo(type, out dataMappingInfos, out referObjs, out propertyInfos);
            dictionary1[type] = dataMappingInfos;
            dictionary2[type] = referObjs;
            dictionary3[type] = propertyInfos;
            EntityBuilder.s_TypeMappingInfo = dictionary1;
            EntityBuilder.s_TypeReferencedList = dictionary2;
            EntityBuilder.s_TypePropertyInfo = dictionary3;
        }

        private class ReferencedTypeBindingInfo
        {
            private ReferencedEntityAttribute m_ReferencedEntityAttribute;
            private PropertyInfo m_PropertyInfo;

            public Type Type
            {
                get
                {
                    return this.m_ReferencedEntityAttribute.Type;
                }
            }

            public string Prefix
            {
                get
                {
                    return this.m_ReferencedEntityAttribute.Prefix;
                }
            }

            public string ConditionalProperty
            {
                get
                {
                    return this.m_ReferencedEntityAttribute.ConditionalProperty;
                }
            }

            public PropertyInfo PropertyInfo
            {
                get
                {
                    return this.m_PropertyInfo;
                }
            }

            public ReferencedTypeBindingInfo(ReferencedEntityAttribute attri, PropertyInfo propertyInfo)
            {
                this.m_ReferencedEntityAttribute = attri;
                this.m_PropertyInfo = propertyInfo;
            }
        }

        private class PropertyDataBindingInfo
        {
            public DataMappingAttribute DataMapping;
            public PropertyInfo PropertyInfo;

            public PropertyDataBindingInfo(DataMappingAttribute mapping, PropertyInfo propertyInfo)
            {
                this.DataMapping = mapping;
                this.PropertyInfo = propertyInfo;
            }
        }
    }
}
