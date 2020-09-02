using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DefaultNamespace.Attributes;
using UnityEngine;


public class AutoAssignManager : MonoBehaviour
{
	private void Awake() {
		MonoBehaviour[] sceneActive = Resources.FindObjectsOfTypeAll<MonoBehaviour>();
		foreach (MonoBehaviour mono in sceneActive) {
			FieldInfo[] objectFields = mono.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			for (int i = 0; i < objectFields.Length; i++) {
				AutoAssign attribute = Attribute.GetCustomAttribute(objectFields[i], typeof(AutoAssign)) as AutoAssign;
				if (attribute != null)
				{
					Type t = objectFields[i].FieldType;
					//Debug.Log(objectFields[i].Name); // The name of the flagged variable.
					Component component;
					switch (attribute.AutoAssignType)
					{
							case AutoAssignType.OnObject:
								component = mono.gameObject.GetComponent(t);
								break;
							case AutoAssignType.OnChild:
								component = mono.gameObject.GetComponentInChildren(t);
								break;
							case AutoAssignType.OnParent:
								component = mono.gameObject.GetComponentInParent(t);
								break;
							case AutoAssignType.Global:
								component = (Component)FindObjectOfType(t);
								break;
							default:
								component = null;
								break;
					}

					if (component == null)
					{
						Debug.LogWarningFormat("Can't autopopulate property {0}:{1} {2} using method {3}", mono.name, objectFields[i].Name, objectFields[i].FieldType, attribute.AutoAssignType);
						continue;
					}
					objectFields[i].SetValue(mono, component);
				}
                        
			}
		}
	}
}