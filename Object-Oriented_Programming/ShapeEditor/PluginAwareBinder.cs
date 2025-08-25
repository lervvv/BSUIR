using System.Runtime.Serialization;
using System;

public class PluginAwareBinder : SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName)
    {
        //загрузка типа из текущей сборки
        Type type = Type.GetType($"{typeName}, {assemblyName}");
        if (type != null) return type;

        //если тип не найден, поиск в загруженных плагинах
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (assembly.FullName == assemblyName ||
                assembly.GetName().Name == assemblyName.Split(',')[0])
            {
                type = assembly.GetType(typeName);
                if (type != null) return type;
            }
        }
        return null;
    }
}