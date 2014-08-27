using Microsoft.Tools.TestClient.Variables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace Microsoft.Tools.TestClient
{
	internal class DataContractAnalyzer : MarshalByRefObject
	{
		internal static IDictionary<string, Type> TypesCache;

		internal static IDictionary<object, Variable> ObjectsCache;

		private static Type[] memberAttributes;

		private static IDictionary<string, SerializableType> serviceTypeInfoPool;

		private static Type[] typeAttributes;

		static DataContractAnalyzer()
		{
			DataContractAnalyzer.TypesCache = new Dictionary<string, Type>();
			DataContractAnalyzer.ObjectsCache = new Dictionary<object, Variable>(new DataContractAnalyzer.EqualityComparer());
			Type[] typeArray = new Type[] { typeof(DataMemberAttribute), typeof(MessageBodyMemberAttribute), typeof(MessageHeaderAttribute), typeof(MessageHeaderArrayAttribute), typeof(XmlAttributeAttribute), typeof(XmlElementAttribute), typeof(XmlArrayAttribute), typeof(XmlTextAttribute) };
			DataContractAnalyzer.memberAttributes = typeArray;
			DataContractAnalyzer.serviceTypeInfoPool = new Dictionary<string, SerializableType>();
			DataContractAnalyzer.typeAttributes = new Type[] { typeof(DataContractAttribute), typeof(XmlTypeAttribute), typeof(MessageContractAttribute) };
		}

		public DataContractAnalyzer()
		{
		}

		internal ClientEndpointInfo AnalyzeDataContract(ClientEndpointInfo endpoint)
		{
			TypeMemberInfo typeMemberInfo;
			Type type = ClientSettings.ClientAssembly.GetType(endpoint.OperationContractTypeName);
			if (type == null)
			{
				endpoint.Valid = false;
				return endpoint;
			}
			object[] customAttributes = type.GetCustomAttributes(typeof(ServiceContractAttribute), true);
			if (customAttributes == null || (int)customAttributes.Length != 1 || !(((ServiceContractAttribute)customAttributes[0]).CallbackContract != null))
			{
				endpoint.Valid = true;
			}
			else
			{
				endpoint.Valid = false;
			}
			endpoint.ClientTypeName = DataContractAnalyzer.GetContractTypeName(type);
			MethodInfo[] methods = type.GetMethods();
			for (int i = 0; i < (int)methods.Length; i++)
			{
				MethodInfo methodInfo = methods[i];
				bool flag = false;
				object[] objArray = methodInfo.GetCustomAttributes(typeof(OperationContractAttribute), false);
				if ((int)objArray.Length == 1 && ((OperationContractAttribute)objArray[0]).IsOneWay)
				{
					flag = true;
				}
				ServiceMethodInfo serviceMethodInfo = new ServiceMethodInfo(endpoint, methodInfo.Name, flag);
				endpoint.Methods.Add(serviceMethodInfo);
				ParameterInfo[] parameters = methodInfo.GetParameters();
				for (int j = 0; j < (int)parameters.Length; j++)
				{
					ParameterInfo parameterInfo = parameters[j];
					string name = parameterInfo.Name;
					typeMemberInfo = (!parameterInfo.ParameterType.IsByRef ? new TypeMemberInfo(name, DataContractAnalyzer.CreateServiceTypeInfo(parameterInfo.ParameterType)) : new TypeMemberInfo(name, DataContractAnalyzer.CreateServiceTypeInfo(parameterInfo.ParameterType.GetElementType())));
					if (parameterInfo.IsIn || !parameterInfo.IsOut)
					{
						serviceMethodInfo.InputParameters.Add(typeMemberInfo);
					}
					else
					{
						serviceMethodInfo.OtherParameters.Add(typeMemberInfo);
					}
				}
				if (methodInfo.ReturnType != null && !methodInfo.ReturnType.Equals(typeof(void)))
				{
					TypeMemberInfo typeMemberInfo1 = new TypeMemberInfo("(return)", DataContractAnalyzer.CreateServiceTypeInfo(methodInfo.ReturnParameter.ParameterType));
					serviceMethodInfo.OtherParameters.Add(typeMemberInfo1);
				}
			}
			return endpoint;
		}

		internal static Variable BuildVariableInfo(string name, object value)
		{
			SerializableType serializableType;
			Variable[] variableArray;
			if (value == null)
			{
				value = new NullObject();
			}
			if (DataContractAnalyzer.ObjectsCache.ContainsKey(value))
			{
				return DataContractAnalyzer.ObjectsCache[value];
			}
			Type type = value.GetType();
			string fullName = type.FullName;
			if (!DataContractAnalyzer.serviceTypeInfoPool.TryGetValue(fullName, out serializableType))
			{
				serializableType = DataContractAnalyzer.CreateServiceTypeInfo(type);
			}
			TypeMemberInfo typeMemberInfo = new TypeMemberInfo(name, serializableType);
			Variable variable = VariableFactory.CreateAssociateVariable(typeMemberInfo, value);
			if (DataContractAnalyzer.ShouldCache(value))
			{
				DataContractAnalyzer.ObjectsCache.Add(value, variable);
			}
			if (typeMemberInfo.Members != null)
			{
				if (type.IsArray)
				{
					Array arrays = (Array)value;
					variableArray = new Variable[arrays.Length];
					for (int i = 0; i < arrays.Length; i++)
					{
						object obj = arrays.GetValue(i);
						variableArray[i] = DataContractAnalyzer.BuildVariableInfo(string.Concat("[", i, "]"), obj);
					}
				}
				else if (DataContractAnalyzer.IsCollectionType(type))
				{
					ICollection collections = (ICollection)value;
					variableArray = new Variable[collections.Count];
					int num = 0;
					foreach (object obj1 in collections)
					{
						int num1 = num;
						num = num1 + 1;
						variableArray[num1] = DataContractAnalyzer.BuildVariableInfo(string.Concat("[", num, "]"), obj1);
					}
				}
				else if (!DataContractAnalyzer.IsDictionaryType(type))
				{
					variableArray = new Variable[typeMemberInfo.Members.Count];
					int num2 = 0;
					PropertyInfo[] properties = type.GetProperties();
					for (int j = 0; j < (int)properties.Length; j++)
					{
						PropertyInfo propertyInfo = properties[j];
						if (DataContractAnalyzer.IsSupportedMember(propertyInfo) || value is DictionaryEntry || DataContractAnalyzer.IsKeyValuePairType(type))
						{
							object obj2 = propertyInfo.GetValue(value, null);
							int num3 = num2;
							num2 = num3 + 1;
							variableArray[num3] = DataContractAnalyzer.BuildVariableInfo(propertyInfo.Name, obj2);
						}
					}
					FieldInfo[] fields = type.GetFields();
					for (int k = 0; k < (int)fields.Length; k++)
					{
						FieldInfo fieldInfo = fields[k];
						if (DataContractAnalyzer.IsSupportedMember(fieldInfo))
						{
							object obj3 = fieldInfo.GetValue(value);
							int num4 = num2;
							num2 = num4 + 1;
							variableArray[num4] = DataContractAnalyzer.BuildVariableInfo(fieldInfo.Name, obj3);
						}
					}
				}
				else
				{
					IDictionary dictionaries = (IDictionary)value;
					variableArray = new Variable[dictionaries.Count];
					int num5 = 0;
					foreach (DictionaryEntry dictionaryEntry in dictionaries)
					{
						int num6 = num5;
						num5 = num6 + 1;
						variableArray[num6] = DataContractAnalyzer.BuildVariableInfo(string.Concat("[", num5, "]"), dictionaryEntry);
					}
				}
				variable.SetChildVariables(variableArray);
			}
			return variable;
		}

		internal static Variable[] BuildVariableInfos(object result, IDictionary<string, object> outValues)
		{
			Variable[] variableArray = new Variable[outValues.Count + 1];
			DataContractAnalyzer.ObjectsCache.Clear();
			variableArray[0] = DataContractAnalyzer.BuildVariableInfo("(return)", result);
			int num = 1;
			foreach (KeyValuePair<string, object> outValue in outValues)
			{
				int num1 = num;
				num = num1 + 1;
				variableArray[num1] = DataContractAnalyzer.BuildVariableInfo(outValue.Key, outValue.Value);
			}
			return variableArray;
		}

		private static SerializableType CreateServiceTypeInfo(Type type)
		{
			string fullName = type.FullName;
			if (DataContractAnalyzer.serviceTypeInfoPool.ContainsKey(type.FullName))
			{
				return DataContractAnalyzer.serviceTypeInfoPool[fullName];
			}
			bool isInvalid = false;
			SerializableType serializableType = new SerializableType(type);
			DataContractAnalyzer.serviceTypeInfoPool.Add(fullName, serializableType);
			if (type.IsArray)
			{
				SerializableType serializableType1 = DataContractAnalyzer.CreateServiceTypeInfo(type.GetElementType());
				isInvalid = serializableType1.IsInvalid;
				serializableType.Members.Add(new TypeMemberInfo("[0]", serializableType1));
			}
			else if (DataContractAnalyzer.IsNullableType(type))
			{
				SerializableType serializableType2 = DataContractAnalyzer.CreateServiceTypeInfo(type.GetGenericArguments()[0]);
				isInvalid = serializableType2.IsInvalid;
				serializableType.Members.Add(new TypeMemberInfo("Value", serializableType2));
			}
			else if (DataContractAnalyzer.IsCollectionType(type))
			{
				Type baseType = type.BaseType;
				if (baseType.IsGenericType)
				{
					SerializableType serializableType3 = DataContractAnalyzer.CreateServiceTypeInfo(baseType.GetGenericArguments()[0]);
					isInvalid = serializableType3.IsInvalid;
					serializableType.Members.Add(new TypeMemberInfo("[0]", serializableType3));
				}
			}
			else if (DataContractAnalyzer.IsDictionaryType(type))
			{
				Type[] genericArguments = type.GetGenericArguments();
				Type type1 = typeof(KeyValuePair<,>);
				Type[] typeArray = new Type[] { genericArguments[0], genericArguments[1] };
				SerializableType serializableType4 = DataContractAnalyzer.CreateServiceTypeInfo(type1.MakeGenericType(typeArray));
				isInvalid = serializableType4.IsInvalid;
				serializableType.Members.Add(new TypeMemberInfo("[0]", serializableType4));
				if (!DataContractAnalyzer.TypesCache.ContainsKey(fullName))
				{
					DataContractAnalyzer.TypesCache.Add(fullName, type);
				}
			}
			else if (DataContractAnalyzer.IsKeyValuePairType(type))
			{
				Type[] genericArguments1 = type.GetGenericArguments();
				SerializableType serializableType5 = DataContractAnalyzer.CreateServiceTypeInfo(genericArguments1[0]);
				SerializableType serializableType6 = DataContractAnalyzer.CreateServiceTypeInfo(genericArguments1[1]);
				isInvalid = (serializableType5.IsInvalid ? true : serializableType6.IsInvalid);
				serializableType.Members.Add(new TypeMemberInfo("Key", serializableType5));
				serializableType.Members.Add(new TypeMemberInfo("Value", serializableType6));
				if (!DataContractAnalyzer.TypesCache.ContainsKey(fullName))
				{
					DataContractAnalyzer.TypesCache.Add(fullName, type);
				}
			}
			else if (DataContractAnalyzer.IsSupportedType(type))
			{
				PropertyInfo[] properties = type.GetProperties();
				for (int i = 0; i < (int)properties.Length; i++)
				{
					PropertyInfo propertyInfo = properties[i];
					if (DataContractAnalyzer.IsSupportedMember(propertyInfo) || type == typeof(DictionaryEntry))
					{
						SerializableType serializableType7 = DataContractAnalyzer.CreateServiceTypeInfo(propertyInfo.PropertyType);
						if (serializableType7.IsInvalid)
						{
							isInvalid = true;
						}
						serializableType.Members.Add(new TypeMemberInfo(propertyInfo.Name, serializableType7));
					}
				}
				FieldInfo[] fields = type.GetFields();
				for (int j = 0; j < (int)fields.Length; j++)
				{
					FieldInfo fieldInfo = fields[j];
					if (DataContractAnalyzer.IsSupportedMember(fieldInfo))
					{
						SerializableType serializableType8 = DataContractAnalyzer.CreateServiceTypeInfo(fieldInfo.FieldType);
						if (serializableType8.IsInvalid)
						{
							isInvalid = true;
						}
						serializableType.Members.Add(new TypeMemberInfo(fieldInfo.Name, serializableType8));
					}
				}
			}
			if (isInvalid)
			{
				serializableType.MarkAsInvalid();
			}
			object[] customAttributes = type.GetCustomAttributes(typeof(KnownTypeAttribute), false);
			for (int k = 0; k < (int)customAttributes.Length; k++)
			{
				KnownTypeAttribute knownTypeAttribute = (KnownTypeAttribute)customAttributes[k];
				serializableType.SubTypes.Add(DataContractAnalyzer.CreateServiceTypeInfo(knownTypeAttribute.Type));
			}
			object[] objArray = type.GetCustomAttributes(typeof(XmlIncludeAttribute), false);
			for (int l = 0; l < (int)objArray.Length; l++)
			{
				XmlIncludeAttribute xmlIncludeAttribute = (XmlIncludeAttribute)objArray[l];
				serializableType.SubTypes.Add(DataContractAnalyzer.CreateServiceTypeInfo(xmlIncludeAttribute.Type));
			}
			return serializableType;
		}

		private static string GetContractTypeName(Type contractType)
		{
			Type[] types = ClientSettings.ClientAssembly.GetTypes();
			for (int i = 0; i < (int)types.Length; i++)
			{
				Type type = types[i];
				if (contractType.IsAssignableFrom(type) && !type.IsInterface)
				{
					return type.FullName;
				}
			}
			return null;
		}

		private static bool HasAttribute(MemberInfo member, Type type)
		{
			return (int)member.GetCustomAttributes(type, true).Length > 0;
		}

		internal static bool IsCollectionType(Type currentType)
		{
			return (int)currentType.GetCustomAttributes(typeof(CollectionDataContractAttribute), true).Length > 0;
		}

		internal static bool IsDataSet(Type type)
		{
			Type type1 = typeof(DataSet);
			if (type.Equals(type1))
			{
				return true;
			}
			return type1.IsAssignableFrom(type);
		}

		internal static bool IsDictionaryType(Type type)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
			{
				return true;
			}
			return false;
		}

		internal static bool IsKeyValuePairType(Type type)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
			{
				return true;
			}
			return false;
		}

		internal static bool IsNullableType(Type type)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return true;
			}
			return false;
		}

		private static bool IsSupportedMember(MemberInfo member)
		{
			Type[] typeArray = DataContractAnalyzer.memberAttributes;
			for (int i = 0; i < (int)typeArray.Length; i++)
			{
				if (DataContractAnalyzer.HasAttribute(member, typeArray[i]))
				{
					return true;
				}
			}
			return false;
		}

		internal static bool IsSupportedType(Type currentType)
		{
			if (currentType == typeof(DictionaryEntry))
			{
				return true;
			}
			Type[] typeArray = DataContractAnalyzer.typeAttributes;
			for (int i = 0; i < (int)typeArray.Length; i++)
			{
				if (DataContractAnalyzer.HasAttribute(currentType, typeArray[i]))
				{
					return true;
				}
			}
			return false;
		}

		private static bool ShouldCache(object value)
		{
			return !(value is string);
		}

		internal class EqualityComparer : IEqualityComparer<object>
		{
			public EqualityComparer()
			{
			}

			public new bool Equals(object x, object y)
			{
				return object.ReferenceEquals(x, y);
			}

			public int GetHashCode(object obj)
			{
				return obj.GetHashCode();
			}
		}
	}
}